using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services;
using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Windows.Controller;
using UnityEngine;

namespace PurpleSlayerFish.Model.Systems
{
    public class LaserProcessor : IRunSystem
    {
        public const string SUBSCRIPTION_LASER_PERFORMED = "player_laser";
        public const string SUBSCRIPTION_LASER_LAUNCHED = "on_laser_launched";
        public const string SUBSCRIPTION_LASER_PREPARE = "on_laser_prepare";
        public const string SUBSCRIPTION_LASER_CANCELED = "on_laser_canceled";
        
        private IEntitiesContext _entitiesContext;
        private IGameConfig _gameConfig;
        private ISubscriptionObserver _subscriptionObserver;
        private MathUtils _mathUtils;
        
        private PlayerEntity _player;
        private LaserEntity _laser;
        private List<IEntity> _lasers;

        public LaserProcessor(IEntitiesContext entitiesContext, IGameConfig gameConfig, ISubscriptionObserver subscriptionObserver)
        {
            _entitiesContext = entitiesContext;
            _gameConfig = gameConfig;
            _subscriptionObserver = subscriptionObserver;
            _mathUtils = new MathUtils();
            subscriptionObserver.Subscribe(SUBSCRIPTION_LASER_PERFORMED, OnLaserPerformed);
        }

        public void Run()
        {
            _player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            TryLaunchLaser();
            CheckLaserLifetime();
            FollowPlayer();
            TryRestoreLaser();
        }
        
        private void OnLaserPerformed()
        {
            if (_player.LaserUsesCount < 1 || _player.LaserTimelapse > 0)
                return;

            _player.LaserDelay = _gameConfig.LaserDelay;
            _subscriptionObserver.Execute(SUBSCRIPTION_LASER_PREPARE);
        }

        private void TryLaunchLaser()
        {
            if (_player.LaserDelay == 0)
                return;
            if (_player.LaserDelay > 0)
            {
                _player.LaserDelay -= Time.deltaTime;
                return;
            }

            _player.LaserDelay = 0;
            _subscriptionObserver.Execute(SUBSCRIPTION_LASER_LAUNCHED);

            _laser = new LaserEntity();
            _laser.Lifetime = _gameConfig.LaserLifeTime;
            _laser.Origin = _player.WorldData.Position;
            _laser.Rotation = _player.WorldData.Rotation;
            _entitiesContext.Insert(LaserEntity.ENTITY_TYPE, _laser);
            _player.LaserTimelapse = _gameConfig.LaserUseCooldown;
            _player.LaserUsesCount--;
            _subscriptionObserver.Execute(GameController.UPDATE_UI_LASER_COUNT, _player.LaserUsesCount);
        }
        
        private void CheckLaserLifetime()
        {
            _player.LaserTimelapse -= Time.deltaTime;
            _laser = _entitiesContext.SelectFirst<LaserEntity>(LaserEntity.ENTITY_TYPE);
            if (_laser == null)
                return;
            _laser.Lifetime -= Time.deltaTime;
            if (_laser.Lifetime < 0)
            {
                _entitiesContext.Remove(LaserEntity.ENTITY_TYPE, _laser);
                _subscriptionObserver.Execute(SUBSCRIPTION_LASER_CANCELED);
            }
        }

        private void FollowPlayer()
        {
            _lasers = _entitiesContext.Select(LaserEntity.ENTITY_TYPE);
            if (_lasers == null)
                return;
            
            for (int i = 0; i < _lasers.Count; i++)
            {
                _laser = (LaserEntity) _lasers[i];
                _laser.Origin = _player.WorldData.Position;
                _laser.Rotation = _player.WorldData.Rotation;
            }
        }

        private void TryRestoreLaser()
        {
            _player.LaserRestorationTime = Mathf.Max(0, _player.LaserRestorationTime - Time.deltaTime);
            if (_player.LaserRestorationTime == 0)
            {
                if (_player.LaserUsesCount < _gameConfig.LaserMaxCount)
                {
                    _player.LaserUsesCount++;
                    _subscriptionObserver.Execute(GameController.UPDATE_UI_LASER_COUNT, _player.LaserUsesCount);
                    _player.LaserRestorationTime = _gameConfig.LaserRestorationTime;
                    if (_player.LaserUsesCount < _gameConfig.LaserMaxCount)
                        _player.LaserRestorationTime = _gameConfig.LaserRestorationTime;
                }
            }
            else
            {
                if (_player.LaserUsesCount == _gameConfig.LaserMaxCount)
                    _player.LaserRestorationTime = _gameConfig.LaserRestorationTime;
                _subscriptionObserver.Execute(GameController.UPDATE_UI_LASER_RESTORATION_TIME, _player.LaserRestorationTime);
            }
        }
    }
}
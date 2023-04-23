using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.Spawners;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Windows.GameWindow;
using PurpleSlayerFish.Model.Entities;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class AlienProcessor : IRunSystem, IInstallSystem
    {
        public const string SUBSCRIPTION_ON_ALIEN_INTERSECT = "on_alien_intersect";
        
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private ILevelBorders _levelBorders;
        [Inject] private AlienSpawner _alienSpawner;
        private MathUtils _mathUtils = new();

        public void Install()
        {
            _subscriptionObserver.Subscribe(SUBSCRIPTION_ON_ALIEN_INTERSECT, new EntitySubscription<AlienEntity>(OnAlienIntersect));
        }

        private float _alienSpawnElapsedTime;
        private AlienEntity _alien;
        private List<IEntity> _aliens;
        private PlayerEntity _player;
        private Vector2 _distance;
        
        public void Run()
        {
            _player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            if (!_player.IsAlive)
                return;
            TryToSpawnAlien();
            ProcessForEachAliens();
        }
        
        private void TryToSpawnAlien()
        {
            _alienSpawnElapsedTime += Time.deltaTime;
            if (_alienSpawnElapsedTime > _gameConfig.AliensSpawnTimelapse)
            {
                _alienSpawnElapsedTime = 0;
                _alien = _alienSpawner.Spawn();
                _alien.FireTimelapse = -3;
                _alien.WorldData.Position = _mathUtils.RandomPerimeterPoint(_levelBorders.OuterBorder0, _levelBorders.OuterBorder1);
                _alien.WorldData.IntersectionOffset = _gameConfig.AlienOffset;
            }
        }
        
        private void ProcessForEachAliens()
        {
            _aliens = _entitiesContext.Select(AlienEntity.ENTITY_TYPE);
            if (_aliens == null)
                return;
            for (int i = 0; i < _aliens.Count; i++)
            {
                _alien = (AlienEntity) _aliens[i];
                MoveAlien(_alien);
                TryAlienFire(_alien);
            }
        }
        
        private void TryAlienFire(AlienEntity alien)
        {
            alien.FireTimelapse += Time.deltaTime;
            if (alien.FireTimelapse > _gameConfig.AliensFireTimelapse)
            {
                alien.FireTimelapse = 0;
                _subscriptionObserver.Execute(BulletProcessor.SUBSCRIPTION_ALIEN_FIRE, alien);
            }
        }

        private void MoveAlien(AlienEntity alien)
        {
            _distance = _player.WorldData.Position - alien.WorldData.Position;
            alien.WorldData.FrameMovement = _distance.sqrMagnitude < Mathf.Pow(_gameConfig.AliensAvoidDistance, 2) 
                ? Vector2.zero 
                : _distance.normalized * _gameConfig.AliensVelocity;
            alien.WorldData.Rotation = 
                _mathUtils.RotateAngleUntil(alien.WorldData.Rotation, Vector2.SignedAngle(Vector2.up, _distance), _gameConfig.AliensFrameMaxRotation);
        }
        
        private void OnAlienIntersect(AlienEntity entity)
        {
            _player.Score += _gameConfig.AlienHitScore;
            _subscriptionObserver.Execute(GameController.UPDATE_UI_SCORE, _player.Score);
            _alienSpawner.Release(entity);
        }
    }
}
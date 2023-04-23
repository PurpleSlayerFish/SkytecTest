using System;
using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Presenter.Services.EffectsManager;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class IntersectionProcessor : IRunSystem
    {
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private EffectsManager _effectsManager;
        
        private MathUtils _mathUtils = new();

        private PlayerEntity _player;
        private AsteroidEntity _asteroid;
        private BulletEntity _bullet;
        private AlienEntity _alien;
        private LaserEntity _laser;
        private List<IEntity> _bullets;
        private List<IEntity> _lasers;
        private IHasWorldData _source;
        private IHasWorldData _target;

        public void Run()
        {
            _player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            _player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            if (!_player.IsAlive)
                return;
            CheckPlayerIntersections();
            CheckPlayerBulletsIntersections();
            TryCheckLaserIntersections();
        }

        private void CheckPlayerIntersections()
        {
            TryCheckIntersection(_entitiesContext.Select(AsteroidEntity.ENTITY_TYPE), _player, AsteroidPlayerIntersection);
            TryCheckIntersection(_entitiesContext.Select(AlienEntity.ENTITY_TYPE), _player, AlienPlayerIntersection);
            
            _bullets = _entitiesContext.Select(BulletEntity.ENTITY_TYPE);
            if (_bullets == null)  
                return;
            _bullets = _bullets.FindAll(bullet => ((BulletEntity) bullet).Hostile);
            TryCheckIntersection(_bullets, _player, BulletPlayerIntersection);
        }
        
        private void CheckPlayerBulletsIntersections()
        {
            _bullets = _entitiesContext.Select(BulletEntity.ENTITY_TYPE);
            if (_bullets == null)  
                return;
            _bullets = _bullets.FindAll(bullet => !((BulletEntity) bullet).Hostile);
            for (int i = 0; i < _bullets.Count; i++)
            {
                TryCheckIntersection(_entitiesContext.Select(AsteroidEntity.ENTITY_TYPE), _bullets[i], AsteroidBulletIntersection);
                TryCheckIntersection(_entitiesContext.Select(AlienEntity.ENTITY_TYPE), _bullets[i], AlienBulletIntersection);
            }
        }
        
        private void TryCheckIntersection(List<IEntity> sources, IEntity target, Action<IEntity, IEntity> onIntersect)
        {
            if (sources == null)
                return;
            for (int i = 0; i < sources.Count; i++)
            {
                _source = (IHasWorldData) sources[i];
                _target = (IHasWorldData) target;
                if ((_target.WorldData.Position - _source.WorldData.Position)
                    .sqrMagnitude > Mathf.Pow(_source.WorldData.IntersectionOffset + _target.WorldData.IntersectionOffset, 2))
                    continue;
                onIntersect.Invoke(sources[i], target);
            }
        }
        
        private void TryCheckLaserIntersections()
        {
            _lasers = _entitiesContext.Select(LaserEntity.ENTITY_TYPE);
            if (_lasers == null)  
                return;
            for (int i = 0; i < _lasers.Count; i++)
            {
                _laser = (LaserEntity)_lasers[i];
                CheckLaserIntersection(_entitiesContext.Select(AsteroidEntity.ENTITY_TYPE), _laser, LaserAsteroidIntersection);
                CheckLaserIntersection(_entitiesContext.Select(AlienEntity.ENTITY_TYPE), _laser, LaserAlienIntersection);
            }
        }

        private void CheckLaserIntersection(List<IEntity> sources, LaserEntity laser, Action<IEntity> onIntersect)
        {
            if (sources == null)
                return;
            for (int i = 0; i < sources.Count; i++)
            {
                _source = (IHasWorldData) sources[i];
                if (CheckRayIntersection(laser.Origin, 
                    laser.Rotation, 
                    _source.WorldData.Position,
                    _gameConfig.LaserOffset + _source.WorldData.IntersectionOffset))
                    onIntersect.Invoke(sources[i]);
            }
        }
        
        public bool CheckRayIntersection(Vector2 rayOrigin, float rayRotation, Vector2 point, float offset)
        {
            Vector2 pointToCircle = point - rayOrigin;
            float dotProduct = Vector2.Dot(pointToCircle, _mathUtils.DirectionFromRotate(rayRotation));
    
            if (dotProduct < 0)
                return false;
            if (pointToCircle.sqrMagnitude - dotProduct * dotProduct > offset * offset)
                return false;
            return true;
        }
        
        private void AsteroidPlayerIntersection(IEntity asteroid, IEntity player)
        {
            _asteroid = (AsteroidEntity) asteroid;
            _player = (PlayerEntity) player;
            _effectsManager.CastExplosion(_player.WorldData.Position);
            _effectsManager.CastExplosion(_asteroid.WorldData.Position);
            _subscriptionObserver.Execute(AsteroidProcessor.SUBSCRIPTION_ON_ASTEROID_INTERSECT, _asteroid);
            _subscriptionObserver.Execute(PlayerInstaller.SUBSCRIPTION_ON_PLAYER_HIT);
        }
        
        private void BulletPlayerIntersection(IEntity bullet, IEntity player)
        {
            _bullet = (BulletEntity) bullet;
            _player = (PlayerEntity) player;
            _effectsManager.CastExplosion(_player.WorldData.Position);
            _subscriptionObserver.Execute(BulletProcessor.SUBSCRIPTION_ON_BULLET_INTERSECT, _bullet);
            _subscriptionObserver.Execute(PlayerInstaller.SUBSCRIPTION_ON_PLAYER_HIT);
        }

        private void AsteroidBulletIntersection(IEntity asteroid, IEntity bullet)
        {
            _asteroid = (AsteroidEntity) asteroid;
            _bullet = (BulletEntity) bullet;
            if (_bullet.View == null)
                return;
            _effectsManager.CastExplosion(_asteroid.WorldData.Position);
            _subscriptionObserver.Execute(AsteroidProcessor.SUBSCRIPTION_ON_ASTEROID_INTERSECT, _asteroid);
            _subscriptionObserver.Execute(BulletProcessor.SUBSCRIPTION_ON_BULLET_INTERSECT, _bullet);
        }
        
        private void AlienBulletIntersection(IEntity alien, IEntity bullet)
        {
            _alien = (AlienEntity) alien;
            _bullet = (BulletEntity) bullet;
            if (_bullet.View == null)
                return;
            _effectsManager.CastExplosion(_alien.WorldData.Position);
            _subscriptionObserver.Execute(AlienProcessor.SUBSCRIPTION_ON_ALIEN_INTERSECT, _alien);
            _subscriptionObserver.Execute(BulletProcessor.SUBSCRIPTION_ON_BULLET_INTERSECT, _bullet);
        }
        
        private void AlienPlayerIntersection(IEntity alien, IEntity player)
        {
            _alien = (AlienEntity) alien;
            _player = (PlayerEntity) player;
            _effectsManager.CastExplosion(_alien.WorldData.Position);
            _effectsManager.CastExplosion(_player.WorldData.Position);
            _subscriptionObserver.Execute(AlienProcessor.SUBSCRIPTION_ON_ALIEN_INTERSECT, _alien);
            _subscriptionObserver.Execute(PlayerInstaller.SUBSCRIPTION_ON_PLAYER_HIT);
        }
        
        private void LaserAsteroidIntersection(IEntity asteroid)
        {
            _asteroid = (AsteroidEntity) asteroid;
            _effectsManager.CastExplosion(_asteroid.WorldData.Position);
            _subscriptionObserver.Execute(AsteroidProcessor.SUBSCRIPTION_ON_ASTEROID_INTERSECT, _asteroid);
        }
        
        private void LaserAlienIntersection(IEntity alien)
        {
            _alien = (AlienEntity) alien;
            _effectsManager.CastExplosion(_alien.WorldData.Position);
            _subscriptionObserver.Execute(AlienProcessor.SUBSCRIPTION_ON_ALIEN_INTERSECT, _alien);
        }
    }
}
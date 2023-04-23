using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.Spawners;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class BulletProcessor : IRunSystem, IInstallSystem
    {
        public const string SUBSCRIPTION_PLAYER_FIRE = "player_fire";
        public const string SUBSCRIPTION_ALIEN_FIRE = "alien_fire";
        public const string SUBSCRIPTION_ON_BULLET_INTERSECT = "on_bullet_intersect";
        
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private BulletSpawner _bulletSpawner;
        private MathUtils _mathUtils = new();
        
        private BulletEntity _tempBullet;

        public void Install()
        {
            _subscriptionObserver.Subscribe(SUBSCRIPTION_PLAYER_FIRE, PlayerFireSubscription);
            _subscriptionObserver.Subscribe<AlienEntity>(SUBSCRIPTION_ALIEN_FIRE, AlienFireSubscription);
            _subscriptionObserver.Subscribe(SUBSCRIPTION_ON_BULLET_INTERSECT, new EntitySubscription<BulletEntity>(OnBulletIntersect));
        }

        public void Run()
        {
            _bulletSpawner.CheckDeathzone();
        }

        private void PlayerFireSubscription() 
        {
            var player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            SpawnBullet(false, player.WorldData.Position, player.WorldData.Rotation);
        }
        
        private void AlienFireSubscription(AlienEntity alien) => SpawnBullet(true, alien.WorldData.Position, alien.WorldData.Rotation + Random.Range(-_gameConfig.AliensFireSpread, _gameConfig.AliensFireSpread));
        private void OnBulletIntersect(BulletEntity entity) => _bulletSpawner.Release(entity);

        private void SpawnBullet(bool hostile, Vector2 position, float rotation)
        {
            _tempBullet = _bulletSpawner.Spawn();
            _tempBullet.Hostile = hostile;
            _tempBullet.WorldData.Position = position;
            _tempBullet.WorldData.Rotation = rotation;
            _tempBullet.WorldData.FrameMovement = _mathUtils.DirectionFromRotate(rotation) * _gameConfig.BulletSpeed;
            _tempBullet.WorldData.IntersectionOffset = _gameConfig.BulletOffset;
        }
    }
}
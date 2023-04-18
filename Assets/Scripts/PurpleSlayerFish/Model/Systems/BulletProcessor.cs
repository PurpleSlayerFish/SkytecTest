using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services;
using PurpleSlayerFish.Model.Services.LevelBorders;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Model.Services.Spawners;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using UnityEngine;

namespace PurpleSlayerFish.Model.Systems
{
    public class BulletProcessor : IRunSystem
    {
        public const string SUBSCRIPTION_PLAYER_FIRE = "player_fire";
        public const string SUBSCRIPTION_ALIEN_FIRE = "alien_fire";
        public const string SUBSCRIPTION_ON_BULLET_INTERSECT = "on_bullet_intersect";
        
        private IEntitiesContext _entitiesContext;
        private IGameConfig _gameConfig;
        private BulletSpawner _bulletSpawner;
        private MathUtils _mathUtils;
        
        private BulletEntity _tempBullet;

        public BulletProcessor(IEntitiesContext entitiesContext, IPoolProvider adaptablePoolProvider, 
            IGameConfig gameConfig, ISubscriptionObserver subscriptionObserver, ILevelBorders levelBorders)
        {
            _entitiesContext = entitiesContext;
            _gameConfig = gameConfig;
            _mathUtils = new MathUtils();
            _bulletSpawner = new BulletSpawner(entitiesContext, adaptablePoolProvider, subscriptionObserver, levelBorders);
            subscriptionObserver.Subscribe(SUBSCRIPTION_PLAYER_FIRE, PlayerFireSubscription);
            subscriptionObserver.Subscribe<AlienEntity>(SUBSCRIPTION_ALIEN_FIRE, AlienFireSubscription);
            subscriptionObserver.Subscribe(SUBSCRIPTION_ON_BULLET_INTERSECT, new EntitySubscription<BulletEntity>(OnBulletIntersect));
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
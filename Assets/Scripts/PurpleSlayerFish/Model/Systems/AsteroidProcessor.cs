using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services;
using PurpleSlayerFish.Model.Services.LevelBorders;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Model.Services.Spawners;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Windows.Controller;
using UnityEngine;

namespace PurpleSlayerFish.Model.Systems
{
    public class AsteroidProcessor : IRunSystem
    {
        public const string SUBSCRIPTION_ON_ASTEROID_INTERSECT = "on_asteroid_intersect";
        
        private IEntitiesContext _entitiesContext;
        private ISubscriptionObserver _subscriptionObserver;
        private IGameConfig _gameConfig;
        private ILevelBorders _levelBorders;
        private AsteroidSpawner _asteroidSpawner;
        private IAsteroidSizeConfig _asteroidSizeConfig;
        private MathUtils _mathUtils;

        private PlayerEntity _player;
        private float _asteroidSpawnElapsedTime;
        private AsteroidSize _size;

        public AsteroidProcessor(IEntitiesContext entitiesContext, IPoolProvider adaptablePoolProvider, 
            IGameConfig gameConfig, ISubscriptionObserver subscriptionObserver, ILevelBorders levelBorders, IAsteroidSizeConfig asteroidSizeConfig)
        {
            _entitiesContext = entitiesContext;
            _subscriptionObserver = subscriptionObserver;
            _gameConfig = gameConfig;
            _levelBorders = levelBorders;
            _asteroidSizeConfig = asteroidSizeConfig;
            _levelBorders.InitAllBorders();
            _mathUtils = new MathUtils();

            _asteroidSpawner = new AsteroidSpawner(entitiesContext, adaptablePoolProvider, subscriptionObserver, _levelBorders, asteroidSizeConfig);
            subscriptionObserver.Subscribe(SUBSCRIPTION_ON_ASTEROID_INTERSECT, new EntitySubscription<AsteroidEntity>(OnAsteroidIntersect));
        }

        public void Run()
        {
            _player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            TryToSpawnAsteroid();
            _asteroidSpawner.CheckDeathzone();
        }
        
        private void TryToSpawnAsteroid()
        {
            _asteroidSpawnElapsedTime += Time.deltaTime;
            if (_asteroidSpawnElapsedTime > _gameConfig.AsteroidsSpawnTime)
                Init(_asteroidSpawner.Spawn(), Random.Range(0, _asteroidSizeConfig.AsteroidSizes.Length), true);
        }

        private void Init(AsteroidEntity entity, int size, bool fromOuter, Vector2 position = default)
        {
            _size = _asteroidSizeConfig.AsteroidSizes[size];
            _asteroidSpawnElapsedTime = 0;
            entity.WorldData.Position = fromOuter ? _mathUtils.RandomPerimeterPoint(_levelBorders.OuterBorder0, _levelBorders.OuterBorder1) : position;
            entity.WorldData.FrameMovement = (fromOuter ? RandomMovementToCenter(entity.WorldData.Position) : RandomMovementShard()) * _size.SpeedMultiplier;
            entity.WorldData.FrameRotation = Random.Range(_gameConfig.AsteroidsRotationFrom, _gameConfig.AsteroidsRotationTo);
            entity.Size = size;
            entity.WorldData.IntersectionOffset = _size.Offset;
        }

        private Vector2 RandomMovementToCenter(Vector2 origin) => 
            (_mathUtils.RandomPerimeterPoint(_levelBorders.InnerBorder0, _levelBorders.InnerBorder1) - origin).normalized
            * Random.Range(_gameConfig.AsteroidsVelocityFrom, _gameConfig.AsteroidsVelocityTo);
        
        private Vector2 RandomMovementShard() => _mathUtils.RandomDirection() * Random.Range(_gameConfig.AsteroidsVelocityFrom, _gameConfig.AsteroidsVelocityTo);

        private void TrySpawnShards(AsteroidEntity entity, ref AsteroidSize asteroidSize)
        {
            // todo outOfBound may produce
            for (int i = 0; i < asteroidSize.SmallShards; i++)
                Init(_asteroidSpawner.Spawn(), entity.Size - 2, false, entity.WorldData.Position);
            for (int i = 0; i < asteroidSize.BigShards; i++)
                Init(_asteroidSpawner.Spawn(), entity.Size - 1, false, entity.WorldData.Position);
        }
        
        private void OnAsteroidIntersect(AsteroidEntity entity)
        {
            _player.Score += _gameConfig.AsteroidHitScore;
            _subscriptionObserver.Execute(GameController.UPDATE_UI_SCORE, _player.Score);
            TrySpawnShards(entity, ref _asteroidSizeConfig.AsteroidSizes[entity.Size]);
            _asteroidSpawner.Release(entity);
        }
    }
}
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Core.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.Spawners;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Windows.GameWindow;
using PurpleSlayerFish.Model.Entities;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class AsteroidProcessor : IRunSystem, IInstallSystem
    {
        public const string SUBSCRIPTION_ON_ASTEROID_INTERSECT = "on_asteroid_intersect";
        
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private ILevelBorders _levelBorders;
        [Inject] private AsteroidSpawner _asteroidSpawner;
        [Inject] private IAsteroidSizeConfig _asteroidSizeConfig;
        private MathUtils _mathUtils = new();

        private PlayerEntity _player;
        private float _asteroidSpawnElapsedTime;
        private AsteroidSize _size;

        public void Install()
        {
            _subscriptionObserver.Subscribe(SUBSCRIPTION_ON_ASTEROID_INTERSECT, new EntitySubscription<AsteroidEntity>(OnAsteroidIntersect));
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
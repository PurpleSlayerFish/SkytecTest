using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Windows.Container;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services;
using PurpleSlayerFish.Model.Services.LevelBorders;
using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Model.Services.Spawners;
using PurpleSlayerFish.View.Services.ParallaxService;
using PurpleSlayerFish.Windows.Controller;
using UnityEngine;

namespace PurpleSlayerFish.Model.Systems
{
    public class LevelProcessor : IRunSystem
    {
        private IEntitiesContext _entitiesContext;
        private IGameConfig _gameConfig;
        private ILevelBorders _levelBorders;
        private IUiContainer _uiContainer;
        private IParallaxService _parallaxService;
        private AsteroidSpawner _asteroidSpawner;
        private MathUtils _mathUtils;

        private PlayerEntity _playerEntity;
        private float _asteroidSpawnElapsedTime;
        
        private AsteroidEntity _tempAsteroid;
        private Vector2 _tempPosition;

        public LevelProcessor(IEntitiesContext entitiesContext, 
            IGameConfig gameConfig, ILevelBorders levelBorders, IUiContainer uiContainer, IParallaxService parallaxService)
        {
            _entitiesContext = entitiesContext;
            _gameConfig = gameConfig;
            _levelBorders = levelBorders;
            _uiContainer = uiContainer;
            _parallaxService = parallaxService;
            _levelBorders.InitAllBorders();
            _mathUtils = new MathUtils();
            _uiContainer.Show<GameController>();
        }

        public void Run()
        {
            TryTeleportPlayer();
            _parallaxService.SetTargetPosition(_playerEntity.WorldData.Position);
        }
        
        private void TryTeleportPlayer()
        {
            _playerEntity = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            if (_levelBorders.RemapBorders(_playerEntity.WorldData.Position, out _tempPosition))
                _playerEntity.WorldData.Position = _tempPosition;
        }
    }
}
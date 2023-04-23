using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.View.Services.ParallaxService;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class LevelProcessor : IRunSystem
    {
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private ILevelBorders _levelBorders;
        [Inject] private IParallaxService _parallaxService;

        private PlayerEntity _playerEntity;
        private float _asteroidSpawnElapsedTime;
        
        private AsteroidEntity _tempAsteroid;
        private Vector2 _tempPosition;

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
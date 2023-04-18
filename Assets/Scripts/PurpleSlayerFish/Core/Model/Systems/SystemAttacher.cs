using PurpleSlayerFish.Core.Windows.Container;
using PurpleSlayerFish.Model.Services.LevelBorders;
using PurpleSlayerFish.Model.Services.PauseService;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using PurpleSlayerFish.Model.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.View.Services.ParallaxService;
using UnityEngine;

namespace PurpleSlayerFish.Core.Model.Systems
{
    public class SystemAttacher : ISystemAttacher
    {
        private IPrefabProvider _prefabProvider;
        private IEntitiesContext _entitiesContext;
        private ISystemManager _systemManager;
        private IPoolProvider _adaptablePoolProvider;
        private IPoolProvider _tempPoolProvider;
        private IGameConfig _gameConfig;
        private ISubscriptionObserver _subscriptionObserver;
        private IPauseService _pauseService;
        private ILevelBorders _levelBorders;
        private Camera _camera;
        private IParallaxService _parallaxService;
        private IAsteroidSizeConfig _asteroidSizeConfig;
        private IUiContainer _uiContainer;

        public SystemAttacher(IPrefabProvider prefabProvider, 
            IEntitiesContext entitiesContext,
            ISystemManager systemManager, 
            IPoolProvider adaptablePoolProvider,
            IPoolProvider tempPoolProvider,
            ISubscriptionObserver subscriptionObserver, 
            IPauseService pauseService,
            IUiContainer uiContainer)
        {
            _prefabProvider = prefabProvider;
            _entitiesContext = entitiesContext;
            _systemManager = systemManager;
            _adaptablePoolProvider = adaptablePoolProvider;
            _tempPoolProvider = tempPoolProvider;
            _subscriptionObserver = subscriptionObserver;
            _pauseService = pauseService;
            _uiContainer = uiContainer;

            _gameConfig = _prefabProvider.Get<GameConfig>(GameProcessor.SCRIPTABLE_OBJECTS_BUNDLE, GameProcessor.GAME_CONFIG_PREFAB);
            _camera = Object.Instantiate(_prefabProvider.Get<GameObject>(GameProcessor.CORE_BUNDLE, GameProcessor.CAMERA_PREFAB)).GetComponent<Camera>();
            _parallaxService = Object.Instantiate(_prefabProvider.Get<GameObject>(GameProcessor.CORE_BUNDLE, GameProcessor.PARALLAX_PREFAB)).GetComponent<EndlessParallaxService>();
            _asteroidSizeConfig = _prefabProvider.Get<AsteroidSizeConfig>(GameProcessor.SCRIPTABLE_OBJECTS_BUNDLE, GameProcessor.ASTEROID_SIZE_CONFIG_PREFAB);
            _levelBorders = new LevelBorders(_camera, _gameConfig);
        }

        public void Attach()
        {
            _systemManager.AttachInitSystem(new PlayerInstaller(_entitiesContext, _prefabProvider, _subscriptionObserver, _gameConfig, _uiContainer));
            _systemManager.AttachInitSystem(new PauseInstaller(_pauseService, _subscriptionObserver));
            _systemManager.AttachUpdateSystem(new LevelProcessor(_entitiesContext, _gameConfig, _levelBorders, _uiContainer, _parallaxService));
            _systemManager.AttachUpdateSystem(new AsteroidProcessor(_entitiesContext, _adaptablePoolProvider, _gameConfig, _subscriptionObserver, _levelBorders, _asteroidSizeConfig));
            _systemManager.AttachUpdateSystem(new AlienProcessor(_entitiesContext, _adaptablePoolProvider, _gameConfig, _subscriptionObserver, _levelBorders));
            _systemManager.AttachUpdateSystem(new BulletProcessor(_entitiesContext, _adaptablePoolProvider,  _gameConfig, _subscriptionObserver, _levelBorders));
            _systemManager.AttachUpdateSystem(new LaserProcessor(_entitiesContext,  _gameConfig, _subscriptionObserver));
            _systemManager.AttachUpdateSystem(new PlayerInputProcessor(_entitiesContext, _gameConfig, _subscriptionObserver));
            _systemManager.AttachUpdateSystem(new DynamicTransformProcessor(_entitiesContext));
            _systemManager.AttachUpdateSystem(new IntersectionProcessor(_entitiesContext, _tempPoolProvider, _gameConfig, _subscriptionObserver));
        }
    }

    public interface ISystemAttacher
    {
        void Attach();
    }
}
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Windows.Container;
using PurpleSlayerFish.Model.Services.PauseService;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Windows.Controller;
using UnityEngine;

namespace PurpleSlayerFish.Core.Model
{
    public class GameProcessor : MonoBehaviour
    {
        public const string CORE_BUNDLE = "core_prefabs";
        public const string SCRIPTABLE_OBJECTS_BUNDLE = "scriptable_objects";
        public const string UI_WINDOWS_BUNDLE = "ui_windows";
        public const string GAME_CONFIG_PREFAB = "GameConfig";
        public const string ASTEROID_SIZE_CONFIG_PREFAB = "AsteroidSizeConfig";
        public const string CAMERA_PREFAB = "[MainCamera]";
        public const string PARALLAX_PREFAB = "[Parallax]";
        public const string TRANSFORM_PREFAB = "Transform";
        public const string UI_PREFAB = "[UI]";

        private ISystemManager _systemManager;
        private ISystemAttacher _systemAttacher;
        private IPrefabProvider _prefabProvider;
        private IEntitiesContext _entitiesContext;
        private IPoolProvider _adaptablePoolProvider;
        private IPoolProvider _tempPoolProvider;
        private ISubscriptionObserver _subscriptionObserver;
        private IUiContainer _uiContainer;

        private void Awake()
        {
            BindDependencies();
            InitializeUi();
            _uiContainer.Get<GameController>();
            InitializeSystems();
        }

        private void Update() => _systemManager.RunUpdateSystems();

        private void OnDestroy()
        {
            _prefabProvider.Dispose();
        }

        private void BindDependencies()
        {
            _systemManager = new SystemManager();
            _prefabProvider = new AssetBundlePrefabProvider();
            _entitiesContext = new GameEntitiesContext();
            _adaptablePoolProvider = new AdaptablePoolProvider(_prefabProvider);
            _tempPoolProvider = new TempPoolProvider(_prefabProvider);
            _subscriptionObserver = new SubscriptionObserver();
        }

        private void InitializeUi() => _uiContainer = new UiContainer(_prefabProvider, _subscriptionObserver);
        
        private void InitializeSystems()
        {
            _systemAttacher = new SystemAttacher(_prefabProvider, _entitiesContext, _systemManager, _adaptablePoolProvider, 
                _tempPoolProvider, _subscriptionObserver, new PauseService(), _uiContainer);
            _systemAttacher.Attach();
            _systemManager.RunInitSystems();
        }
    }
}

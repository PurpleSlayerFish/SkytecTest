using PurpleSlayerFish.Core.Global;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Core.Services.PauseService;
using PurpleSlayerFish.Core.Services.Pools.PoolProvider;
using PurpleSlayerFish.Core.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.GameWindow;
using PurpleSlayerFish.Core.Ui.Windows.PauseWindow;
using PurpleSlayerFish.Presenter.Services.EffectsManager;
using PurpleSlayerFish.View.Services.ParallaxService;
using UnityEngine;
using Zenject;
using IPrefabProvider = PurpleSlayerFish.Core.Services.PrefabProvider.IPrefabProvider;

namespace PurpleSlayerFish.Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject] private IPrefabProvider _prefabProvider;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private IUiContainer _uiContainer;

        public override void InstallBindings()
        {
            _subscriptionObserver.Clear();
            
            BindInterfaces();
            BindInstances();
            BindUi();
            Container.BindInterfacesTo<SystemManager>().AsSingle();
        }

        private void BindInterfaces()
        {
            Container.BindInterfacesTo<GameEntitiesContext>().AsSingle();
            Container.BindInterfacesTo<AdaptablePoolProvider>().AsSingle();
            Container.BindInterfacesTo<EffectsPoolProvider>().AsSingle();
            Container.BindInterfacesTo<LevelBorders>().AsSingle();
            Container.BindInterfacesTo<PauseService>().AsSingle();
        }

        private void BindInstances()
        {
            Container.Bind<IGameConfig>().FromInstance(_prefabProvider.Get<GameConfig>(GameGlobal.SCRIPTABLE_OBJECTS_BUNDLE, GameGlobal.GAME_CONFIG_PREFAB)).AsSingle();
            Container.Bind<IAsteroidSizeConfig>().FromInstance(_prefabProvider.Get<AsteroidSizeConfig>(GameGlobal.SCRIPTABLE_OBJECTS_BUNDLE, GameGlobal.ASTEROID_SIZE_CONFIG_PREFAB)).AsSingle();
            Container.Bind<IParallaxService>().FromInstance(Instantiate(_prefabProvider.GetComponent<EndlessParallaxService>(GameGlobal.CORE_BUNDLE, GameGlobal.PARALLAX_PREFAB))).AsSingle();
            Container.BindInstance(Instantiate(_prefabProvider.GetComponent<Camera>(GameGlobal.CORE_BUNDLE, GameGlobal.CAMERA_PREFAB))).AsSingle();
            Container.BindInstance(Container.Instantiate<EffectsManager>());
        }

        private void BindUi()
        {
            _uiContainer.ClearRoot();
            _uiContainer.InitializeWindow<GameWindow>();
            _uiContainer.InitializeWindow<PauseWindow>();
            _uiContainer.Show<GameController>();
        }
    }
}

using PurpleSlayerFish.Core.Data;
using PurpleSlayerFish.Core.Services.DataStorage;
using PurpleSlayerFish.Core.Services.PrefabProvider;
using PurpleSlayerFish.Core.Services.Purchases;
using PurpleSlayerFish.Core.Services.SceneLoader;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.ElementManager;
using Zenject;
using IPrefabProvider = PurpleSlayerFish.Core.Services.PrefabProvider.IPrefabProvider;

namespace PurpleSlayerFish.Core.Installers
{
    public class ProjectBindInstaller : MonoInstaller
    {
        private AssetBundlePrefabProvider _prefabProvider;
        public override void InstallBindings()
        {
            _prefabProvider = new AssetBundlePrefabProvider(Container);
            Container.Bind<IPrefabProvider>().FromInstance(_prefabProvider).AsSingle();
            Container.BindInterfacesTo<UiElementManager>().AsSingle();
            Container.BindInterfacesTo<UiContainer>().AsSingle();
            Container.BindInterfacesTo<SubscriptionObserver>().AsSingle();
            Container.BindInterfacesTo<AsyncSceneLoader>().AsSingle();
            Container.BindInterfacesTo<TestPurchaseService>().AsSingle();
            Container.BindInterfacesTo<PlayerPrefsStorage<PlayerData>>().AsSingle();
        }
        
        private void OnDestroy()
        {
            _prefabProvider.Dispose();;
        }
    }
}
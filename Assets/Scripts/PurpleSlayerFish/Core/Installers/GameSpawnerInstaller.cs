using PurpleSlayerFish.Core.Services.Pools.PoolProvider;
using PurpleSlayerFish.Core.Services.Spawners;
using Zenject;

namespace PurpleSlayerFish.Core.Installers
{
    public class GameSpawnerInstaller : MonoInstaller
    {
        [Inject] private IAdaptablePoolProvider _adaptablePoolProvider;
        [Inject] private IEffectsPoolProvider _effectsPoolProvider;
        public override void InstallBindings()
        {
            _adaptablePoolProvider.InitPools();
            _effectsPoolProvider.InitPools();
            var alienSpawner = Container.Instantiate<AlienSpawner>();
            var asteroidSpawner = Container.Instantiate<AsteroidSpawner>();
            var bulletSpawner = Container.Instantiate<BulletSpawner>();
            alienSpawner.Init(Container);
            asteroidSpawner.Init(Container);
            bulletSpawner.Init(Container);
            
            Container.BindInstance(alienSpawner).AsSingle();
            Container.BindInstance(asteroidSpawner).AsSingle();
            Container.BindInstance(bulletSpawner).AsSingle();
            // Container.BindInterfacesTo<AsteroidSpawner>().AsSingle();
            // Container.BindInterfacesTo<BulletSpawner>().AsSingle();
        }
    }
}

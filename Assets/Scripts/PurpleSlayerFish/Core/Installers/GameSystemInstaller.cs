using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Model.Systems;
using Zenject;

namespace PurpleSlayerFish.Core.Installers
{
    public class GameSystemInstaller : MonoInstaller
    {
        [Inject] private ILevelBorders _levelBorders;
        [Inject] private ISystemManager _systemManager;
        public override void InstallBindings()
        {
            _levelBorders.InitAllBorders();
            var alienProcessor = Bind<AlienProcessor>();
            var bulletProcessor = Bind<BulletProcessor>();
            var laserProcessor = Bind<LaserProcessor>();
            var asteroidProcessor = Bind<AsteroidProcessor>();
            _systemManager.AttachInitSystem(Bind<PlayerInstaller>());
            _systemManager.AttachInitSystem(Bind<PauseInstaller>());
            _systemManager.AttachInitSystem(alienProcessor);
            _systemManager.AttachInitSystem(bulletProcessor);
            _systemManager.AttachInitSystem(laserProcessor);
            _systemManager.AttachInitSystem(asteroidProcessor);
            _systemManager.AttachUpdateSystem(Bind<LevelProcessor>());
            _systemManager.AttachUpdateSystem(asteroidProcessor);
            _systemManager.AttachUpdateSystem(alienProcessor);
            _systemManager.AttachUpdateSystem(bulletProcessor);
            _systemManager.AttachUpdateSystem(laserProcessor);
            _systemManager.AttachUpdateSystem(Bind<PlayerInputProcessor>());
            _systemManager.AttachUpdateSystem(Bind<DynamicTransformProcessor>());
            _systemManager.AttachUpdateSystem(Bind<IntersectionProcessor>());
            _systemManager.RunInitSystems();
        }

        private T Bind<T>() => Container.Instantiate<T>();
    }
}
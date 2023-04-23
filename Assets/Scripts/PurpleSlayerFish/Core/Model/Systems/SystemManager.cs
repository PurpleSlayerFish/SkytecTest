using System.Collections.Generic;
using Zenject;

namespace PurpleSlayerFish.Core.Model.Systems
{
    public class SystemManager : ISystemManager, ITickable
    {
        private List<IInstallSystem> _initSystems;
        private List<IRunSystem> _updateSystems;

        public SystemManager()
        {
            _initSystems = new List<IInstallSystem>();
            _updateSystems = new List<IRunSystem>();
        }
        
        public void RunInitSystems() => InstallSystems(_initSystems);
        public void AttachInitSystem(IInstallSystem system) => AttachSystem(system, _initSystems);
        public void AttachUpdateSystem(IRunSystem system) => AttachSystem(system, _updateSystems);
        private void AttachSystem<T>(T system, List<T> systems) where T : ISystem => systems.Add(system);
        
        private void InstallSystems(List<IInstallSystem> systems)
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Install();
        }
        
        public void Tick()
        {
            for (int i = 0; i < _updateSystems.Count; i++)
                _updateSystems[i].Run();
        }
    }

    public interface ISystemManager
    {
        void RunInitSystems();
        void AttachInitSystem(IInstallSystem system);
        void AttachUpdateSystem(IRunSystem system);
    }
}
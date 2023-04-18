using System.Collections.Generic;

namespace PurpleSlayerFish.Core.Model.Systems
{
    public class SystemManager : ISystemManager
    {
        private List<IInstallSystem> _initSystems;
        private List<IRunSystem> _updateSystems;

        public SystemManager()
        {
            _initSystems = new List<IInstallSystem>();
            _updateSystems = new List<IRunSystem>();
        }
        
        public void RunInitSystems() => InstallSystems(_initSystems);
        public void RunUpdateSystems() => RunSystems(_updateSystems);
        public void AttachInitSystem<T>() where T : IInstallSystem, new() => AttachSystem(new T(), _initSystems);
        public void AttachUpdateSystem<T>() where T : IRunSystem, new() => AttachSystem(new T(), _updateSystems);
        public void AttachInitSystem(IInstallSystem system) => AttachSystem(system, _initSystems);
        public void AttachUpdateSystem(IRunSystem system) => AttachSystem(system, _updateSystems);
        
        private void InstallSystems(List<IInstallSystem> systems)
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Install();
        }
        
        private void RunSystems(List<IRunSystem> systems)
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Run();
        }

        private void AttachSystem<T>(T system, List<T> systems) where T : ISystem => systems.Add(system);
    }

    public interface ISystemManager
    {
        void RunInitSystems();
        void RunUpdateSystems();
        void AttachInitSystem<T>() where T : IInstallSystem, new();
        void AttachUpdateSystem<T>() where T : IRunSystem, new();
        void AttachInitSystem(IInstallSystem system);
        void AttachUpdateSystem(IRunSystem system);
    }
}
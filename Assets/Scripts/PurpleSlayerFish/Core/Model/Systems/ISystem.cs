namespace PurpleSlayerFish.Core.Model.Systems
{
    public interface ISystem
    {
    }

    public interface IRunSystem : ISystem
    {
        void Run();
    }

    public interface IInstallSystem : ISystem
    {
        void Install();
    }
}
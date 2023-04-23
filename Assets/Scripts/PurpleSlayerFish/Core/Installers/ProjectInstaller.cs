using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.LoadingWindow;
using Zenject;

namespace PurpleSlayerFish.Core.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [Inject] private IUiContainer _uiContainer;

        public override void InstallBindings()
        {
            _uiContainer.Install(Container);
            _uiContainer.InitializeWindow<LoadingWindow>(false);
        }
    }
}
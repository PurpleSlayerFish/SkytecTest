using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.MainMenuWindow;
using PurpleSlayerFish.Core.Ui.Windows.ShopWindow;
using Zenject;

namespace PurpleSlayerFish.Core.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [Inject] private IUiContainer _uiContainer;
        
        public override void InstallBindings()
        {
            _uiContainer.ClearRoot();
            _uiContainer.InitializeWindow<MainMenuWindow>();
            _uiContainer.InitializeWindow<ShopWindow>();
            _uiContainer.Show<MainMenuController>();
        }
    }
}
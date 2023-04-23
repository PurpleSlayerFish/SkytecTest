using PurpleSlayerFish.Core.Services.SceneLoader;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.ShopWindow;
using Zenject;

namespace PurpleSlayerFish.Core.Ui.Windows.MainMenuWindow
{
    public class MainMenuController : AbstractController<MainMenuWindow>
    {
        [Inject] private IUiContainer _uiContainer;
        [Inject] private ISceneLoader _sceneLoader;
        
        protected override void AfterInitialize()
        {
            _window.PlayBtn.AddOnClick(LoadGame);
            _window.ShopBtn.AddOnClick(() => _uiContainer.Show<ShopController>());
            _window.QuitBtn.AddOnClick(BuildQuitDialog);
        }

        private void LoadGame() => _sceneLoader.Load(_window.GameSceneName);

        private void BuildQuitDialog()
        {
            _uiContainer.BuildDialog()
                .WithLabel("Quit")
                .WithDescription("Are you sure want to quit the game?")
                .WithButton("Yes!", () => Quit())
                .WithButton("No!", () => SetInteractable(true), true)
                .Build()
                .Show();

            SetInteractable(false);

            void Quit()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                UnityEngine.Application.Quit();
#endif
            }
        }
    }
}
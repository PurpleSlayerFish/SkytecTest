using PurpleSlayerFish.Core.Services.SceneLoader;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Model.Systems;
using Zenject;

namespace PurpleSlayerFish.Core.Ui.Windows.PauseWindow
{
    public class PauseController : AbstractController<PauseWindow>
    {
        [Inject] private IUiContainer _uiContainer;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private ISubscriptionObserver _subscriptionObserver;

        protected override void AfterInitialize()
        {
            _window.PlayButton.AddOnClick(() => _subscriptionObserver.Execute(PauseInstaller.SUBSCRIPTION_PAUSE));
            _window.QuitButton.AddOnClick(Quit);
        }

        private void Quit()
        {
            _uiContainer.BuildDialog()
                .WithLabel("Quit?")
                .WithDescription("Go to the main menu?")
                .WithButton("Yes!", () =>
                {
                    _subscriptionObserver.Execute(PauseInstaller.SUBSCRIPTION_PAUSE);
                    _sceneLoader.Load(_window.MainMenuScene);
                })
                .WithButton("No!", () => SetInteractable(true), true)
                .Build()
                .Show();
            
            SetInteractable(false);
        }
    }
}
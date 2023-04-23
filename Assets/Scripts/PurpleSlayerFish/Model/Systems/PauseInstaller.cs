using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services.PauseService;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.PauseWindow;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class PauseInstaller : IInstallSystem
    {
        public const string SUBSCRIPTION_PAUSE = "pause";
        
        [Inject] private IPauseService _pauseService;
        // todo переделать контейнер
        [Inject] private IUiContainer _uiContainer;
        [Inject] private ISubscriptionObserver _subscriptionObserver;

        public void Install() => _subscriptionObserver.Subscribe(SUBSCRIPTION_PAUSE, new SimpleSubscription(ProcessPause));

        private void ProcessPause()
        {
            _pauseService.IsPaused = !_pauseService.IsPaused;
            Time.timeScale = _pauseService.IsPaused ? 0 : 1;
            
            if (_pauseService.IsPaused)
                _uiContainer.Show<PauseController>();
            else
                _uiContainer.Hide<PauseController>();
        }
    }
}
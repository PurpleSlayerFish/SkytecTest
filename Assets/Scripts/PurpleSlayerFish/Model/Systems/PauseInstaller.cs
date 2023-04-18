using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Model.Services.PauseService;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using UnityEngine;

namespace PurpleSlayerFish.Model.Systems
{
    public class PauseInstaller : IInstallSystem
    {
        public const string SUBSCRIPTION_PAUSE = "pause";
        
        private IPauseService _pauseService;
        private ISubscriptionObserver _subscriptionObserver;
        
        public PauseInstaller(IPauseService pauseService, ISubscriptionObserver subscriptionObserver)
        {
            _pauseService = pauseService;
            _subscriptionObserver = subscriptionObserver;
        }

        public void Install() => _subscriptionObserver.Subscribe(SUBSCRIPTION_PAUSE, new SimpleSubscription(ProcessPause));

        private void ProcessPause()
        {
            _pauseService.IsPaused = !_pauseService.IsPaused;
            Time.timeScale = _pauseService.IsPaused ? 0 : 1;
            Debug.Log("Pause: " + _pauseService.IsPaused);
        }
    }
}
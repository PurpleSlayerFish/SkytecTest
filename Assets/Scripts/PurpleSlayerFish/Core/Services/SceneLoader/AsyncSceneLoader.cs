using System.Threading.Tasks;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.LoadingWindow;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace PurpleSlayerFish.Core.Services.SceneLoader
{
    public class AsyncSceneLoader : ISceneLoader
    {
        [Inject] private IUiContainer _uiContainer;

        private float _simulatedLoadingTime = 2.5f;
        
        private float _startTime;
        private LoadingController _loadingController;
        
        public async void Load(string sceneName)
        {
            _loadingController = _uiContainer.Get<LoadingController>();
            _loadingController.Show();
            
            await Task.Delay(Mathf.RoundToInt(_loadingController.FadeDuration * 1000));
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;

            _startTime = Time.time;
            while (asyncOperation.progress < 0.9f && Time.time - _startTime < _simulatedLoadingTime)
                await Task.Yield();
            
            if (Time.time - _startTime < _simulatedLoadingTime)
                await Task.Delay(Mathf.FloorToInt((_simulatedLoadingTime - (Time.time - _startTime)) * 1000));

            _loadingController.Hide();
            asyncOperation.allowSceneActivation = true;
        }
    }
}
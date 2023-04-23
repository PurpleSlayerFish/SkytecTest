using System.Threading.Tasks;
using PurpleSlayerFish.Core.Services.SceneLoader;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.SplashWindow;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Core.Installers
{
    public class SplashInstaller : MonoInstaller
    {
        [SerializeField] private int _delayMilliseconds = 1500;
        [SerializeField] private string _nextSceneName = "MainMenuScene";
        
        [Inject] private IUiContainer _uiContainer;
        [Inject] private ISceneLoader _sceneLoader;

        public override void InstallBindings()
        {
            _uiContainer.InitializeWindow<SplashWindow>();
            _uiContainer.Show<SplashController>();
            DelayLoad();
        }
        
        public async void DelayLoad()
        {
            await Task.Delay(_delayMilliseconds);
            _sceneLoader.Load(_nextSceneName);
        }
    }
}
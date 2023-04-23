using PurpleSlayerFish.Core.Ui.ElementManager.Elements;
using UnityEngine;

namespace PurpleSlayerFish.Core.Ui.Windows.MainMenuWindow
{
    public class MainMenuWindow : AbstractWindow<MainMenuController>
    {
        [Header("Buttons")]
        [SerializeField] private ExtendedButton _playBtn;
        public ExtendedButton PlayBtn => _playBtn;
        [SerializeField] private ExtendedButton _shopBtn;
        public ExtendedButton ShopBtn => _shopBtn;
        [SerializeField] private ExtendedButton quitBtn;
        public ExtendedButton QuitBtn => quitBtn;
        
        [SerializeField] private string _gameSceneName = "GameScene";
        public string GameSceneName => _gameSceneName;
    }
}
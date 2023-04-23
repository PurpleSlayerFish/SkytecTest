using PurpleSlayerFish.Core.Ui.ElementManager.Elements;
using UnityEngine;

namespace PurpleSlayerFish.Core.Ui.Windows.PauseWindow
{
    public class PauseWindow : AbstractWindow<PauseController>
    {
        [Header("Quit")]
        [SerializeField] private string _mainMenuScene = "MainMenuScene";
        public string MainMenuScene => _mainMenuScene;
        [SerializeField] private ExtendedButton _quitButton;
        public ExtendedButton QuitButton => _quitButton;
        [SerializeField] private ExtendedButton _playButton;
        public ExtendedButton PlayButton => _playButton;
    }
}
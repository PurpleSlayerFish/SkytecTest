using UnityEngine;

namespace PurpleSlayerFish.Core.Ui.Container
{
    public class UiProvider : MonoBehaviour
    {
        [SerializeField] private Canvas _rootCanvas;
        [SerializeField] private Camera _uiCamera;
        public Canvas RootCanvas => _rootCanvas;
        public Camera UiCamera => _uiCamera;
    }
}
using UnityEngine;

namespace PurpleSlayerFish.Core.Ui.Windows.LoadingWindow
{
    public class LoadingWindow : AbstractWindow<LoadingController>
    {
        [Header("Fade")]
        [SerializeField] private float _fadeDuration = 0.7f;
        public float FadeDuration => _fadeDuration;
        
        [Header("Loading Wheel")]
        [SerializeField] private RectTransform _loadingWheel;
        public RectTransform LoadingWheel => _loadingWheel;
        [SerializeField] private float _rotationSpeed = -200f;
        public float RotationSpeed => _rotationSpeed;
    }
}
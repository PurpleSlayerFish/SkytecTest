using System.Threading.Tasks;
using UnityEngine;

namespace PurpleSlayerFish.Core.Ui.Windows.LoadingWindow
{
    public class LoadingController : AbstractController<LoadingWindow>
    {
        private float _currentTime;
        public float FadeDuration => _window.FadeDuration;

        protected override async Task DynamicShow() => await Fade(0, 1);
        protected override async Task DynamicHide() => await Fade(1, 0);

        public override async void Show()
        {
            base.Show();
            await RotateLoadingWheel();
        }

        private async Task Fade(float from, float to)
        {
            if (_window == null)
                return;
            _window.CanvasGroup.alpha = from;
            _currentTime = 0;
            while ( _currentTime < _window.FadeDuration)
            {
                _window.CanvasGroup.alpha = Mathf.Abs(from - _currentTime / _window.FadeDuration);
                _currentTime += Time.deltaTime;
                await Task.Yield();
            }
            _window.CanvasGroup.alpha = to;
        }

        private async Task RotateLoadingWheel()
        {
            while (_window != null && _window.Canvas.enabled)
            {
                _window.LoadingWheel.Rotate(0f, 0f, _window.RotationSpeed * Time.deltaTime);
                await Task.Yield();
            }
        }
    }
}
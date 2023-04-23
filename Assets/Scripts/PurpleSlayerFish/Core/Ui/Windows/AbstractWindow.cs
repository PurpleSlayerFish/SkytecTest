using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Core.Ui.Windows
{
    public abstract class AbstractWindow<T> : AbstractWindow where T : AbstractController
    {
        private T _controller;
        public T0 Initialize<T0>(DiContainer _container) where T0 : T => Initialize(_container) as T0;
        public override AbstractController Initialize(DiContainer _container)
        {
            _controller = _container.Instantiate<T>();
            _controller.Initialize(this);
            return _controller;
        }

        private void OnDestroy()
        {
            _controller = null;
        }
    }

    public abstract class AbstractWindow : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] protected Canvas _canvas;
        public Canvas Canvas => _canvas;
        [SerializeField] protected CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public abstract AbstractController Initialize(DiContainer _container);
    }
}
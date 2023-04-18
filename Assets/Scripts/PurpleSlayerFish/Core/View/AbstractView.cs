using PurpleSlayerFish.Core.Presenter;
using UnityEngine;

namespace PurpleSlayerFish.Core.View
{
    public abstract class AbstractView : MonoBehaviour
    {
    }
    
    public abstract class AbstractView<T> : AbstractView where T : AbstractView<T>
    {
        public IPresenter Presenter;
        public delegate void AfterInit(ViewEvent<T> afterInitEvent);
        public delegate void BeforeDestroy(ViewEvent<T> beforeDestroyEvent);
        public delegate void AfterEnable(ViewEvent<T> onEnableEvent);
        public delegate void BeforeDisable(ViewEvent<T> onDisableEvent);
        public event AfterInit OnAfterInitEvent;
        public event BeforeDestroy OnBeforeDestroyEvent;
        public event AfterEnable OnEnableEvent;
        public event BeforeDisable OnDisableEvent;

        private void Start() => OnAfterInitEvent?.Invoke(new ViewEvent<T>(this as T));
        private void OnDestroy()
        {
            OnBeforeDestroyEvent?.Invoke(new ViewEvent<T>(this as T));
            Presenter.Dispose();
        }

        private void OnEnable() => OnEnableEvent?.Invoke(new ViewEvent<T>(this as T));
        private void OnDisable() => OnDisableEvent?.Invoke(new ViewEvent<T>(this as T));

        public class ViewEvent<T> where T : AbstractView<T>
        {
            private readonly T _view;
            public T View => _view;

            public ViewEvent(T view)
            {
                _view = view;
            }
        }
    }
}
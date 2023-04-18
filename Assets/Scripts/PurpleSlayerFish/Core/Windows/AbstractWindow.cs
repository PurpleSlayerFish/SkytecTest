using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using UnityEngine;

namespace PurpleSlayerFish.Core.Windows
{
    public abstract class AbstractWindow<T> : AbstractWindow where T : AbstractController, new()
    {
        protected AbstractController _controller;
        public T0 Initialize<T0>(ISubscriptionObserver subscriptionObserver) where T0 : T => Initialize(subscriptionObserver) as T0;
        public override AbstractController Initialize(ISubscriptionObserver subscriptionObserver)
        {
            _controller = new T();
            _controller.Initialize(this, subscriptionObserver);
            return _controller;
        }
    }

    public abstract class AbstractWindow : MonoBehaviour
    {
        [SerializeField] protected Canvas _canvas;
        public Canvas Canvas => _canvas;

        public abstract AbstractController Initialize(ISubscriptionObserver subscriptionObserver);
    }
}
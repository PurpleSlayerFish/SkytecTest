using PurpleSlayerFish.Model.Services.SubscriptionObserver;

namespace PurpleSlayerFish.Core.Windows
{
    public abstract class AbstractController<T> : AbstractController where T : AbstractWindow
    {
        protected ISubscriptionObserver _subscriptionObserver;
        protected T _window;
        
        public override void Initialize(AbstractWindow window, ISubscriptionObserver subscriptionObserver)
        {
            _window = (T) window;
            _subscriptionObserver = subscriptionObserver;
            Hide();
            AfterInitialize();
        }

        public override void Show() => _window.Canvas.enabled = true;
        public override void Hide() => _window.Canvas.enabled = false;
    }

    public abstract class AbstractController
    {
        public abstract void Initialize(AbstractWindow window, ISubscriptionObserver subscriptionObserver);
        public abstract void Show();
        public abstract void Hide();

        protected virtual void AfterInitialize(){}
        
        // todo dispose
    }
}
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.View;

namespace PurpleSlayerFish.Core.Presenter
{
    public abstract class AbstractPresenter<T1, T2> : IPresenter
        where T1 : class, IEntity
        where T2 : AbstractView<T2>
    {
        protected ISubscriptionObserver _subscriptionObserver;
        
        protected T1 _entity;
        protected T2 _view;
        
        protected AbstractPresenter(T1 entity, T2 view, ISubscriptionObserver subscriptionObserver)
        {
            _subscriptionObserver = subscriptionObserver;
            _view = view;
            _view.Presenter = this;
            ReloadEntity(entity);
        }

        public virtual void Dispose()
        {
            UnsubscribeToEvents();
            _subscriptionObserver = null;
            _entity = null;
            _view.Presenter = null;
            _view = null;
        }

        public virtual void ReloadEntity(T1 entity)
        {
            _entity = entity;
            UnsubscribeToEvents();
            SubscribeToEvents();
        }

        public virtual void SubscribeToEvents() {}
        public virtual void UnsubscribeToEvents() {}
    }
}
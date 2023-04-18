using PurpleSlayerFish.Core.Presenter;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Presenter.Utils;
using PurpleSlayerFish.View.Views;

namespace PurpleSlayerFish.Presenter.Presenters
{
    public class AlienPresenter : AbstractPresenter<AlienEntity, AlienView>
    {
        private ITransformSynchronizer _transformSynchronizer;
        public AlienPresenter(AlienEntity entity, AlienView view, ISubscriptionObserver subscriptionObserver) : base(entity, view, subscriptionObserver)
        {
        }
        
        public override void ReloadEntity(AlienEntity entity)
        {
            base.ReloadEntity(entity);
            _transformSynchronizer = new TransformSynchronizer2D(entity, _view.transform);
        }

        public override void Dispose()
        {
            base.Dispose();
            _transformSynchronizer.Dispose();
            _transformSynchronizer = null;
        }
    }
}
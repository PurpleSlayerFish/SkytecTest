using PurpleSlayerFish.Core.Presenter;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Presenter.Utils;
using PurpleSlayerFish.View.Views;

namespace PurpleSlayerFish.Presenter.Presenters
{
    public class BulletPresenter : AbstractPresenter<BulletEntity, BulletView>
    {
        private ITransformSynchronizer _transformSynchronizer;
        public BulletPresenter(BulletEntity entity, BulletView view, ISubscriptionObserver subscriptionObserver) : base(entity, view, subscriptionObserver)
        {
        }

        public override void ReloadEntity(BulletEntity entity)
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
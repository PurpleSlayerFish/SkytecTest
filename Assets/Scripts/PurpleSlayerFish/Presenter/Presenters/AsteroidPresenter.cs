using PurpleSlayerFish.Core.Presenter;
using PurpleSlayerFish.Core.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Presenter.Utils;
using PurpleSlayerFish.View.Views;

namespace PurpleSlayerFish.Presenter.Presenters
{
    public class AsteroidPresenter : AbstractPresenter<AsteroidEntity, AsteroidView>
    {
        private IAsteroidSizeConfig _asteroidSizeConfig;
        private ITransformSynchronizer _transformSynchronizer;
        public AsteroidPresenter(AsteroidEntity entity, AsteroidView view, ISubscriptionObserver subscriptionObserver, IAsteroidSizeConfig asteroidSizeConfig) : base(entity, view, subscriptionObserver)
        {
            _asteroidSizeConfig = asteroidSizeConfig;
        }

        public override void ReloadEntity(AsteroidEntity entity)
        {
            base.ReloadEntity(entity);
            _transformSynchronizer = new TransformSynchronizer2D(entity, _view.transform);
        }

        public override void SubscribeToEvents() => _entity.OnSizeChange += SetSizeSprite;
        public override void UnsubscribeToEvents() => _entity.OnSizeChange -= SetSizeSprite;
        private void SetSizeSprite() => _view.Renderer.sprite = _asteroidSizeConfig.AsteroidSizes[_entity.Size].Sprite;

        public override void Dispose()
        {
            base.Dispose();
            _transformSynchronizer.Dispose();
            _transformSynchronizer = null;
        }
    }
}
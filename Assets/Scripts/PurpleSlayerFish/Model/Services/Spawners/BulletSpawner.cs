using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services.LevelBorders;
using PurpleSlayerFish.Model.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;

namespace PurpleSlayerFish.Model.Services.Spawners
{
    public class BulletSpawner : ISpawner<BulletEntity>
    {
        public const string BULLET_PREFAB = "Bullet";
        
        private IEntitiesContext _entitiesContext;
        private PoolAdapter<BulletEntity, BulletView> _bulletAdapter;
        private ISubscriptionObserver _subscriptionObserver;
        private MathUtils _mathUtils;
        private ILevelBorders _levelBorders;

        private BulletEntity _tempBullet;
        private List<IEntity> _tempEntities;

        public BulletSpawner(IEntitiesContext entitiesContext, IPoolProvider poolProvider,
            ISubscriptionObserver subscriptionObserver, ILevelBorders levelBorders)
        {
            _entitiesContext = entitiesContext;
            _subscriptionObserver = subscriptionObserver;
            _mathUtils = new MathUtils();
            _levelBorders = levelBorders;
            _bulletAdapter =
                new PoolAdapter<BulletEntity, BulletView>(poolProvider, BULLET_PREFAB, LinkViewModel);
        }
        
        private BulletEntity LinkViewModel(BulletView view)
        {
            BulletEntity entity = new BulletEntity();
            entity.View = view;
            if (view.Presenter == null)
                new BulletPresenter(entity, view, _subscriptionObserver);
            else
                ((BulletPresenter) view.Presenter).ReloadEntity(entity);
            return entity;
        }

        public BulletEntity Spawn()
        {
            _tempBullet = _bulletAdapter.Get();
            _entitiesContext.Insert(BulletEntity.ENTITY_TYPE, _tempBullet);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _tempBullet);
            return _tempBullet;
        }
        
        public void Release(BulletEntity entity)
        {
            _entitiesContext.Remove(BulletEntity.ENTITY_TYPE, entity);
            _entitiesContext.Remove(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, entity);
            _bulletAdapter.Release(entity);
        }
        
        public void CheckDeathzone()
        {
            _tempEntities = _entitiesContext.Select(BulletEntity.ENTITY_TYPE);
            if (_tempEntities == null)
                return;
            
            for (int i = 0; i < _tempEntities.Count; i++)
            {
                _tempBullet = _tempEntities[i] as BulletEntity;
                if (_mathUtils.OverlapRectangle(_tempBullet.WorldData.Position, _levelBorders.Border0, _levelBorders.Border1))
                    continue;
                Release(_tempBullet);
            }
        }
    }
}
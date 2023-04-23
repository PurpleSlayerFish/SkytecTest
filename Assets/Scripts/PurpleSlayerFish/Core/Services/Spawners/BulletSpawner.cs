using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Core.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Core.Services.Pools.PoolProvider;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;
using Zenject;

namespace PurpleSlayerFish.Core.Services.Spawners
{
    public class BulletSpawner : ISpawner<BulletEntity>
    {
        private const string BULLET_PREFAB = "Bullet";
        
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private ILevelBorders _levelBorders;
        [Inject] private IAdaptablePoolProvider _poolProvider;
        private PoolAdapter<BulletEntity, BulletView> _adapter;
        private MathUtils _mathUtils = new();

        private BulletEntity _tempBullet;
        private List<IEntity> _tempEntities;

        public void Init(DiContainer container)
        {
            _adapter = container.Instantiate<PoolAdapter<BulletEntity, BulletView>>();
            _adapter.LinkViewModelFunc = LinkViewModel;
            _adapter.PoolKey = BULLET_PREFAB;
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
            _tempBullet = _adapter.Get();
            _entitiesContext.Insert(BulletEntity.ENTITY_TYPE, _tempBullet);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _tempBullet);
            return _tempBullet;
        }
        
        public void Release(BulletEntity entity)
        {
            _entitiesContext.Remove(BulletEntity.ENTITY_TYPE, entity);
            _entitiesContext.Remove(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, entity);
            _adapter.Release(entity);
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
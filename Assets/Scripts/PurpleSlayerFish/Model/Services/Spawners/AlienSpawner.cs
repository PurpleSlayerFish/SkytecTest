using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;

namespace PurpleSlayerFish.Model.Services.Spawners
{
    public class AlienSpawner : ISpawner<AlienEntity>
    {
        public const string ALIEN_PREFAB = "Alien";
        
        private IEntitiesContext _entitiesContext;
        private PoolAdapter<AlienEntity, AlienView> _alienAdapter;
        private ISubscriptionObserver _subscriptionObserver;

        private AlienEntity _tempEntity;
        private List<IEntity> _tempEntities;

        public AlienSpawner(IEntitiesContext entitiesContext, IPoolProvider poolProvider,
            ISubscriptionObserver subscriptionObserver)
        {
            _entitiesContext = entitiesContext;
            _subscriptionObserver = subscriptionObserver;
            _alienAdapter =
                new PoolAdapter<AlienEntity, AlienView>(poolProvider, ALIEN_PREFAB, LinkViewModel);
        }
        
        private AlienEntity LinkViewModel(AlienView view)
        {
            AlienEntity entity = new AlienEntity();
            entity.View = view;
            if (view.Presenter == null)
                new AlienPresenter(entity, view, _subscriptionObserver);
            else
                ((AlienPresenter) view.Presenter).ReloadEntity(entity);
            return entity;
        }
        
        public AlienEntity Spawn()
        {
            _tempEntity = _alienAdapter.Get();
            _entitiesContext.Insert(AlienEntity.ENTITY_TYPE, _tempEntity);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _tempEntity);
            return _tempEntity;
        }
        
        public void Release(AlienEntity entity)
        {
            _entitiesContext.Remove(AlienEntity.ENTITY_TYPE, entity);
            _entitiesContext.Remove(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, entity);
            _alienAdapter.Release(entity);
        }
    }
}
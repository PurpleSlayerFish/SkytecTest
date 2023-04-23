using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;
using Zenject;

namespace PurpleSlayerFish.Core.Services.Spawners
{
    public class AlienSpawner : ISpawner<AlienEntity>
    {
        public const string ALIEN_PREFAB = "Alien";
        
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        private PoolAdapter<AlienEntity, AlienView> _adapter;

        private AlienEntity _tempEntity;
        private List<IEntity> _tempEntities;

        public void Init(DiContainer container)
        {
            _adapter = container.Instantiate<PoolAdapter<AlienEntity, AlienView>>();
            _adapter.LinkViewModelFunc = LinkViewModel;
            _adapter.PoolKey = ALIEN_PREFAB;
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
            _tempEntity = _adapter.Get();
            _entitiesContext.Insert(AlienEntity.ENTITY_TYPE, _tempEntity);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _tempEntity);
            return _tempEntity;
        }
        
        public void Release(AlienEntity entity)
        {
            _entitiesContext.Remove(AlienEntity.ENTITY_TYPE, entity);
            _entitiesContext.Remove(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, entity);
            _adapter.Release(entity);
        }

        public void CheckDeathzone()
        {
            throw new System.NotImplementedException();
        }
    }
}
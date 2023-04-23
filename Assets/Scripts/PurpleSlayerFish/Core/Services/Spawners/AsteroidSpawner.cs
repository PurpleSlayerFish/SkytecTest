using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.LevelBorders;
using PurpleSlayerFish.Core.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Core.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;
using Zenject;

namespace PurpleSlayerFish.Core.Services.Spawners
{
    public class AsteroidSpawner : ISpawner<AsteroidEntity>
    {
        public const string ASTEROID_PREFAB = "Asteroid";
        
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private ILevelBorders _levelBorders;
        [Inject] private IAsteroidSizeConfig _asteroidSizeConfig;
        private PoolAdapter<AsteroidEntity, AsteroidView> _adapter;
        private MathUtils _mathUtils = new();

        private AsteroidEntity _tempEntity;
        private List<IEntity> _tempEntities;
        
        public void Init(DiContainer container)
        {
            _adapter = container.Instantiate<PoolAdapter<AsteroidEntity, AsteroidView>>();
            _adapter.LinkViewModelFunc = LinkViewModel;
            _adapter.PoolKey = ASTEROID_PREFAB;
        }
        
        private AsteroidEntity LinkViewModel(AsteroidView view)
        {
            AsteroidEntity entity = new AsteroidEntity();
            entity.View = view;
            if (view.Presenter == null)
                new AsteroidPresenter(entity, view, _subscriptionObserver, _asteroidSizeConfig);
            else
                ((AsteroidPresenter) view.Presenter).ReloadEntity(entity);
            return entity;
        }
        
        public AsteroidEntity Spawn()
        {
            _tempEntity = _adapter.Get();
            _entitiesContext.Insert(AsteroidEntity.ENTITY_TYPE, _tempEntity);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _tempEntity);
            return _tempEntity;
        }
        
        public void Release(AsteroidEntity entity)
        {
            _entitiesContext.Remove(AsteroidEntity.ENTITY_TYPE, entity);
            _entitiesContext.Remove(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, entity);
            _adapter.Release(entity);
        }
        
        public void CheckDeathzone()
        {
            _tempEntities = _entitiesContext.Select(AsteroidEntity.ENTITY_TYPE);
            if (_tempEntities == null)
                return;
            
            for (int i = 0; i < _tempEntities.Count; i++)
            {
                _tempEntity = _tempEntities[i] as AsteroidEntity;
                if (_mathUtils.OverlapRectangle(_tempEntity.WorldData.Position, _levelBorders.DeathzoneBorder0, _levelBorders.DeathzoneBorder1))
                    continue;
                Release(_tempEntity);
            }
        }
    }
}
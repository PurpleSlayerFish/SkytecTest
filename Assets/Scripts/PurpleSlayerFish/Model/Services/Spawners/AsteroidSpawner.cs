using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services.LevelBorders;
using PurpleSlayerFish.Model.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using PurpleSlayerFish.Model.Services.ScriptableObjects.AsteroidSize;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;

namespace PurpleSlayerFish.Model.Services.Spawners
{
    public class AsteroidSpawner : ISpawner<AsteroidEntity>
    {
        public const string ASTEROID_PREFAB = "Asteroid";
        
        private IEntitiesContext _entitiesContext;
        private PoolAdapter<AsteroidEntity, AsteroidView> _asteroidAdapter;
        private ISubscriptionObserver _subscriptionObserver;
        private MathUtils _mathUtils;
        private ILevelBorders _levelBorders;
        private IAsteroidSizeConfig _asteroidSizeConfig;

        private AsteroidEntity _tempEntity;
        private List<IEntity> _tempEntities;

        public AsteroidSpawner(IEntitiesContext entitiesContext, IPoolProvider poolProvider,
            ISubscriptionObserver subscriptionObserver, ILevelBorders levelBorders, IAsteroidSizeConfig asteroidSizeConfig)
        {
            _entitiesContext = entitiesContext;
            _subscriptionObserver = subscriptionObserver;
            _levelBorders = levelBorders;
            _asteroidSizeConfig = asteroidSizeConfig;
            _mathUtils = new MathUtils();
            _asteroidAdapter =
                new PoolAdapter<AsteroidEntity, AsteroidView>(poolProvider, ASTEROID_PREFAB, LinkViewModel);
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
            _tempEntity = _asteroidAdapter.Get();
            _entitiesContext.Insert(AsteroidEntity.ENTITY_TYPE, _tempEntity);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _tempEntity);
            return _tempEntity;
        }
        
        public void Release(AsteroidEntity entity)
        {
            _entitiesContext.Remove(AsteroidEntity.ENTITY_TYPE, entity);
            _entitiesContext.Remove(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, entity);
            _asteroidAdapter.Release(entity);
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
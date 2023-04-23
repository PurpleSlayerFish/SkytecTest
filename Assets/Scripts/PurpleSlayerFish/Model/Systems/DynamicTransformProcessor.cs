using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class DynamicTransformProcessor : IRunSystem
    {
        public const string DYNAMIC_TRANSFORM_ENTITY_TYPE = "dynamic_transform";
        
        [Inject] private IEntitiesContext _entitiesContext;

        private List<IEntity> _entities;
        private IHasWorldData _tempEntity;

        public void Run()
        {
            _entities = _entitiesContext.Select(DYNAMIC_TRANSFORM_ENTITY_TYPE);
            for (int i = 0; i < _entities.Count; i++)
            {
                _tempEntity = (IHasWorldData) _entities[i];
                if (_tempEntity.WorldData.FrameMovement != Vector2.zero)
                    _tempEntity.WorldData.Position += _tempEntity.WorldData.FrameMovement * Time.deltaTime;
                if (_tempEntity.WorldData.FrameRotation != 0)
                    _tempEntity.WorldData.Rotation += _tempEntity.WorldData.FrameRotation * Time.deltaTime;
            }
        }
    }
}
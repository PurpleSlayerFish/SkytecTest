using PurpleSlayerFish.Core.Model;
using UnityEngine;

namespace PurpleSlayerFish.Presenter.Utils
{
    public class TransformSynchronizer2D : ITransformSynchronizer
    {
        private IHasWorldData _entity;
        private Transform _transform;

        public TransformSynchronizer2D(IHasWorldData entity, Transform transform)
        {
            _entity = entity;
            _transform = transform;
            Subscribe();
        }

        private void Subscribe()
        {
            _entity.WorldData.OnPositionChange += UpdateTransformPosition;
            _entity.WorldData.OnRotationChange += UpdateTransformRotation;
        }
        
        private void Unsubscribe()
        {
            _entity.WorldData.OnPositionChange -= UpdateTransformPosition;
            _entity.WorldData.OnRotationChange -= UpdateTransformRotation;
        }
        
        private void UpdateTransformPosition()
        {
            if (!_transform.gameObject.activeInHierarchy)
                return;
            _transform.position =
                new Vector3(_entity.WorldData.Position.x, _entity.WorldData.Position.y, _transform.position.z);
        }

        private void UpdateTransformRotation()
        {
            if (!_transform.gameObject.activeInHierarchy)
                return;
            _transform.eulerAngles =
                new Vector3(_transform.eulerAngles.x, _transform.eulerAngles.y, _entity.WorldData.Rotation);
        }

        public void Dispose()
        {
            Unsubscribe();
            _entity = null;
            _transform = null;
        }
    }
}
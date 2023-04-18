using System;
using UnityEngine;

namespace PurpleSlayerFish.View.Services.ParallaxService
{
    public class EndlessParallaxService : MonoBehaviour, IParallaxService
    {
        [SerializeField] private EndlessLayer _layer;

        private Vector3 _targetPosition;
        private Vector3 _prevPosition;
        private float _aspectRatio;

        private void Start() => _aspectRatio = (float) Screen.height / Screen.width;
        private void Update() => Process();

        private void Process()
        {
            _layer.Transform.position =
                new Vector3(
                    _layer.Transform.position.x -
                    (_targetPosition - _prevPosition).x * _layer.ParallaxScaleX * _aspectRatio,
                    _layer.Transform.position.y -
                    (_targetPosition - _prevPosition).y * _layer.ParallaxScaleY * _aspectRatio,
                    _layer.Transform.position.z);
            _prevPosition = _targetPosition;
        }

        public void SetTargetPosition(Vector3 position) => _targetPosition = position;

        [Serializable]
        public class EndlessLayer
        {
            [SerializeField] private Transform _transform;
            public Transform Transform => _transform;
            [SerializeField] private float _parallaxScaleX;
            public float ParallaxScaleX => _parallaxScaleX;
            [SerializeField] private float _parallaxScaleY;
            public float ParallaxScaleY => _parallaxScaleY;
        }
    }
}
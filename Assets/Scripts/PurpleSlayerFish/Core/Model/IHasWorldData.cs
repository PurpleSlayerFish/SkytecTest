using System;
using UnityEngine;

namespace PurpleSlayerFish.Core.Model
{
    public interface IHasWorldData : IEntity
    {
        ref WorldData WorldData { get; }
    }

    public struct WorldData
    {
        private Vector2 _position;
        private float _rotation;
        public Vector2 FrameMovement { get; set; }
        public float FrameRotation { get; set; }
        public Action OnPositionChange { get; set; }
        public Action OnRotationChange { get; set; }
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPositionChange?.Invoke();
            }
        }
        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                OnRotationChange?.Invoke();
            }
        }
        public float IntersectionOffset { get; set; }
    }
}
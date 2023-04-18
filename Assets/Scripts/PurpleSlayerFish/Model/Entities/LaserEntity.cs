using PurpleSlayerFish.Core.Model;
using UnityEngine;

namespace PurpleSlayerFish.Model.Entities
{
    public class LaserEntity : IEntity
    {
        public const string ENTITY_TYPE = "laser";

        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public float Lifetime { get; set; }
    }
}
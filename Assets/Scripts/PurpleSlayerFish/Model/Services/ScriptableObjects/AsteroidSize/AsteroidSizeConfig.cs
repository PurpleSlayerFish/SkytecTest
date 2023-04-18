using System;
using UnityEngine;

namespace PurpleSlayerFish.Model.Services.ScriptableObjects.AsteroidSize
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AsteroidSizeConfig", fileName = "AsteroidSizeConfig")]
    public class AsteroidSizeConfig : ScriptableObject, IAsteroidSizeConfig
    {
        [SerializeField] private AsteroidSize[] _asteroidSizes;
        public AsteroidSize[] AsteroidSizes => _asteroidSizes;
    }

    [Serializable]
    public struct AsteroidSize
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _offset;
        [SerializeField] private float _speedMultiplier;
        [SerializeField] private int _bigShards;
        [SerializeField] private int _smallShards;
        
        public Sprite Sprite => _sprite;
        public float Offset => _offset;
        public float SpeedMultiplier => _speedMultiplier;
        public int BigShards => _bigShards;
        public int SmallShards => _smallShards;
    }
}
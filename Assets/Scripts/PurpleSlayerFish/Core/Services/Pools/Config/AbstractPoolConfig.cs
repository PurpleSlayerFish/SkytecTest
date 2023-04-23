using System;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.Pools.Config
{
    public abstract class AbstractPoolConfig<T> : ScriptableObject, IPoolConfig<T> where T : PoolData
    {
        public abstract T[] GetData { get; }
    }
    
    public interface IPoolConfig<T> where T : PoolData
    {
        T[] GetData { get; }
    }

    [Serializable]
    public class PoolData
    {
        [SerializeField] private string _prefabName;
        public string PrefabName => _prefabName;
        [SerializeField] private string _bundleName;
        public string BundleName => _bundleName;
        [SerializeField] private int _defaultCapacity;
        public int DefaultCapacity => _defaultCapacity;
        [SerializeField] private int _maxPoolSize;
        public int MaxPoolSize => _maxPoolSize;
    }
}
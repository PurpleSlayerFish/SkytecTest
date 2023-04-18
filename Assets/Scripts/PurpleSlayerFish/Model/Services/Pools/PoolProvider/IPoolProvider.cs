using PurpleSlayerFish.Core.View;
using UnityEngine;
using UnityEngine.Pool;

namespace PurpleSlayerFish.Model.Services.Pools.PoolProvider
{
    public interface IPoolProvider
    {
        AbstractView Get(string poolKey);
        void Release(string poolKey, AbstractView view);
    }
    
    public class TransformPool
    {
        public Transform Transform { get; }
        public IObjectPool<AbstractView> ObjectPool { get; }

        public TransformPool(Transform transform, IObjectPool<AbstractView> objectPool)
        {
            Transform = transform;
            ObjectPool = objectPool;
        }
    }
}
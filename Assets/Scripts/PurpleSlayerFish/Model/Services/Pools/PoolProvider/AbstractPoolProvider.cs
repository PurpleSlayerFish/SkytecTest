using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.View;
using PurpleSlayerFish.Model.Services.Pools.Config;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using UnityEngine;
using UnityEngine.Pool;

namespace PurpleSlayerFish.Model.Services.Pools.PoolProvider
{
    public abstract class AbstractPoolProvider<T> : IPoolProvider where T : PoolData
    {
        protected abstract string PoolerConfigName { get; }
        protected abstract string RootName { get; }

        // todo добавить массив кешированных префабов/компонентов, который будет хранить их для инстатиэйта в CreatePooledItem
        protected Dictionary<string, TransformPool> _pools;
        protected IPrefabProvider _prefabProvider;
        protected IPoolConfig<T> PoolConfig;
        protected Transform _rootTransform;

        protected Transform _tempTransform;
        protected GameObject _tempGameObject;
        protected AbstractView _tempView;

        public AbstractPoolProvider(IPrefabProvider prefabProvider)
        {
            _pools = new Dictionary<string, TransformPool>();
            _prefabProvider = prefabProvider;
            PoolConfig = _prefabProvider.Get<AbstractPoolConfig<T>>(GameProcessor.SCRIPTABLE_OBJECTS_BUNDLE, PoolerConfigName);
            _rootTransform = InstantiateTransform();
            _rootTransform.name = RootName;
            InitPools();
        }

        private void InitPools()
        {
            T data;
            for (int i = 0; i < PoolConfig.GetData.Length; i++)
            {
                data = PoolConfig.GetData[i];
                _pools.Add(data.PrefabName, CreateTransformPool(data));
            }
        }

        private TransformPool CreateTransformPool(in T data)
        {
            _tempTransform = InstantiateTransform();
            _tempTransform.name = data.PrefabName;
            _tempTransform.SetParent(_rootTransform);
            var cachedData = data;
            var cachedParent = _tempTransform;
            return new TransformPool(_tempTransform,
                new ObjectPool<AbstractView>(
                    () => CreatePooledItem(cachedParent, cachedData),
                    OnTakeFromPool,
                    view => OnReturnedToPool(view),
                    OnDestroyPoolObject,
                    false,
                    data.DefaultCapacity,
                    data.MaxPoolSize));
        }

        protected abstract AbstractView CreatePooledItem(Transform parent, T data);
        protected virtual void OnTakeFromPool(AbstractView view) => view.gameObject.SetActive(true);
        private void OnReturnedToPool(AbstractView view)
        {
            if (!Application.isPlaying)
                return;
            view.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(AbstractView view) => Object.Destroy(view.gameObject);
        private Transform InstantiateTransform() =>
            Object.Instantiate(_prefabProvider.Get<GameObject>(GameProcessor.CORE_BUNDLE, GameProcessor.TRANSFORM_PREFAB)).transform;

        public AbstractView Get(string poolKey) => _pools[poolKey].ObjectPool.Get();
        public void Release(string poolKey, AbstractView view) => _pools[poolKey].ObjectPool.Release(view);
    }
}
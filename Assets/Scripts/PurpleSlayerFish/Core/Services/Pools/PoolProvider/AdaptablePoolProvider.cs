using PurpleSlayerFish.Core.Services.Pools.Config;
using PurpleSlayerFish.Core.View;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.Pools.PoolProvider
{
    public class AdaptablePoolProvider : AbstractPoolProvider<PoolData>, IAdaptablePoolProvider
    {
        protected override string PoolerConfigName => "AdaptablePoolConfig";
        protected override string RootName => "[AdaptablePoolProvider]";

        protected override AbstractView CreatePooledItem(Transform parent, PoolData data)
        {
            _tempGameObject = Object.Instantiate(_prefabProvider.Get<GameObject>(data.BundleName, data.PrefabName));
            _tempGameObject.transform.SetParent(parent);
            _tempView = _tempGameObject.GetComponent<AbstractView>();
            return _tempView;
        }
    }
    
    public interface IAdaptablePoolProvider : IPoolProvider
    {
    
    }
}
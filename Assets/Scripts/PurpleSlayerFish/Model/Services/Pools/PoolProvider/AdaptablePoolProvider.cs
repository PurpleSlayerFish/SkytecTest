using PurpleSlayerFish.Core.View;
using PurpleSlayerFish.Model.Services.Pools.Config;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using UnityEngine;

namespace PurpleSlayerFish.Model.Services.Pools.PoolProvider
{
    public class AdaptablePoolProvider : AbstractPoolProvider<PoolData>
    {
        protected override string PoolerConfigName => "AdaptablePoolConfig";
        protected override string RootName => "[AdaptablePoolProvider]";

        public AdaptablePoolProvider(IPrefabProvider prefabProvider) : base(prefabProvider)
        {
        }

        protected override AbstractView CreatePooledItem(Transform parent, PoolData data)
        {
            _tempGameObject = Object.Instantiate(_prefabProvider.Get<GameObject>(data.BundleName, data.PrefabName));
            _tempGameObject.transform.SetParent(parent);
            _tempView = _tempGameObject.GetComponent<AbstractView>();
            return _tempView;
        }
    }
}
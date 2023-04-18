using PurpleSlayerFish.Core.View;
using PurpleSlayerFish.Model.Services.Pools.Config;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using UnityEngine;

namespace PurpleSlayerFish.Model.Services.Pools.PoolProvider
{
    public class TempPoolProvider :  AbstractPoolProvider<TempPoolData>
    {
        protected override string PoolerConfigName => "TempPoolConfig";
        protected override string RootName => "[TempPoolProvider]";

        public TempPoolProvider(IPrefabProvider prefabProvider) : base(prefabProvider)
        {
        }
        
        protected override AbstractView CreatePooledItem(Transform parent, TempPoolData data)
        {
            _tempGameObject = Object.Instantiate(_prefabProvider.Get<GameObject>(data.BundleName, data.PrefabName));
            _tempGameObject.transform.SetParent(parent);
            var temporaryView = _tempGameObject.GetComponent<TemporaryView>();
            temporaryView.Temporator = new Temporator(data.LifeTimeMilliseconds, () => Release(data.PrefabName, temporaryView));
            _tempView = temporaryView;
            return _tempView;
        }
        
        protected override void OnTakeFromPool(AbstractView view)
        {
            base.OnTakeFromPool(view);
            ((TemporaryView) view).Temporator.Execute();
        }
    }
}
using PurpleSlayerFish.Core.Services.Pools.Config;
using PurpleSlayerFish.Core.View;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.Pools.PoolProvider
{
    public class EffectsPoolProvider :  AbstractPoolProvider<TempPoolData>, IEffectsPoolProvider
    {
        // todo rename TempPoolConfig and TempPoolData
        protected override string PoolerConfigName => "TempPoolConfig";
        protected override string RootName => "[TempPoolProvider]";

        protected override AbstractView CreatePooledItem(Transform parent, TempPoolData data)
        {
            _tempGameObject = Object.Instantiate(_prefabProvider.Get<GameObject>(data.BundleName, data.PrefabName));
            _tempGameObject.transform.SetParent(parent);
            var temporaryView = _tempGameObject.GetComponent<EffectView>();
            temporaryView.Temporator = new Temporator(data.LifeTimeMilliseconds, () => Release(data.PrefabName, temporaryView));
            _tempView = temporaryView;
            return _tempView;
        }
        
        protected override void OnTakeFromPool(AbstractView view)
        {
            base.OnTakeFromPool(view);
            ((EffectView) view).Temporator.Execute();
        }
    }
    
    public interface IEffectsPoolProvider : IPoolProvider
    {
    
    }
}
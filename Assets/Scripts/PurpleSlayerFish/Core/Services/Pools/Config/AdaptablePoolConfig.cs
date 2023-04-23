using UnityEngine;

namespace PurpleSlayerFish.Core.Services.Pools.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AdaptablePoolConfig", fileName = "AdaptablePoolConfig")]
    public class AdaptablePoolConfig : AbstractPoolConfig<PoolData>
    {
        [SerializeField] private PoolData[] _poolData;
        public override PoolData[] GetData => _poolData;
    }
}
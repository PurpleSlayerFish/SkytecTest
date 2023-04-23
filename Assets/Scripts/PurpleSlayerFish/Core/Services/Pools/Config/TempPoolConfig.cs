using System;
using UnityEngine;

namespace PurpleSlayerFish.Core.Services.Pools.Config
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TempPoolConfig", fileName = "TempPoolConfig")]
    public class TempPoolConfig : AbstractPoolConfig<TempPoolData>
    {
        [SerializeField] private TempPoolData[] _poolData;
        public override TempPoolData[] GetData => _poolData;
    }

    [Serializable]
    public class TempPoolData : PoolData
    {
        [SerializeField] private int _lifeTimeMilliseconds;
        public int LifeTimeMilliseconds => _lifeTimeMilliseconds;
    }
}
using PurpleSlayerFish.Model.Services.Pools.PoolProvider;
using UnityEngine;

namespace PurpleSlayerFish.Presenter.Services.EffectsManager
{
    public class EffectsManager
    {
        private const string EXPLOSION_PREFAB = "Explosion";
        private readonly IPoolProvider _poolProvider;

        private Transform _tempTransform;

        public EffectsManager(IPoolProvider poolProvider)
        {
            _poolProvider = poolProvider;
        }

        public void CastExplosion(Vector2 position)
        {
            _tempTransform = _poolProvider.Get(EXPLOSION_PREFAB).transform;
            _tempTransform.position = new Vector3(position.x, position.y, _tempTransform.position.z);
        }
    }
}
using System;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Core.View;

namespace PurpleSlayerFish.Model.Entities
{
    public class AsteroidEntity : IPoolableEntity, IHasWorldData
    {
        public const string ENTITY_TYPE = "asteroid";
        
        private WorldData _worldData;
        public ref WorldData WorldData => ref _worldData;

        public AbstractView View { get; set; }

        private int _size;
        public Action OnSizeChange { get; set; }
        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                OnSizeChange?.Invoke();
            }
        }
    }
}
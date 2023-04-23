using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Services.Pools.PoolAdapter;
using PurpleSlayerFish.Core.View;

namespace PurpleSlayerFish.Model.Entities
{
    public class AlienEntity : IPoolableEntity, IHasWorldData
    {
        public const string ENTITY_TYPE = "alien";

        public float FireTimelapse { get; set; }

        private WorldData _worldData;
        public ref WorldData WorldData => ref _worldData;
        
        public AbstractView View { get; set; }
    }
}
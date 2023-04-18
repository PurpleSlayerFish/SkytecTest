using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.View;
using PurpleSlayerFish.Model.Services.Pools.PoolAdapter;

namespace PurpleSlayerFish.Model.Entities
{
    public class BulletEntity : IPoolableEntity, IHasWorldData
    {
        public const string ENTITY_TYPE = "bullet";
        
        public bool Hostile { get; set; }

        private WorldData _worldData;
        public ref WorldData WorldData => ref _worldData;
        
        public AbstractView View { get; set; }
    }
}
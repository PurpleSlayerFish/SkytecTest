using PurpleSlayerFish.Core.Model;

namespace PurpleSlayerFish.Model.Entities
{
    public class PlayerEntity : IHasWorldData
    {
        public const string ENTITY_TYPE = "player";

        public float MovementDuration { get; set; }
        public float MovementMagnitude { get; set; }
        public float RotationDuration { get; set; }
        public float FireTimelapse { get; set; }
        public float LaserTimelapse { get; set; }
        public float LaserDelay { get; set; }
        public float LaserRestorationTime { get; set; }
        public int LaserUsesCount { get; set; }
        public float LastRotation { get; set; }
        public int Score { get; set; }
        public bool IsAlive { get; set; }

        private WorldData _worldData;
        public ref WorldData WorldData => ref _worldData;
        public bool IsMovePerformed { get; set; }
        public float PerformedRotation { get; set; }
        public bool IsFirePerformed { get; set; }
        public bool IsLaserUsed { get; set; }
        public bool IsEscapeUsed { get; set; }
    }
}
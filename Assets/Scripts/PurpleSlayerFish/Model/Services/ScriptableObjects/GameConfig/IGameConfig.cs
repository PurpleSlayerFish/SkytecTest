namespace PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig
{
    public interface IGameConfig
    {
        float MovementTime { get; }
        float RotationTime { get; }
        float MovementVelocity { get; }
        float RotationVelocity { get; }
        float LaserLifeTime { get; }
        float LaserUseCooldown { get; }
        float LaserDelay { get; }
        int LaserMaxCount { get; }
        float LaserRestorationTime { get; }
        
        float OuterBorderOffset { get; }
        float InnerBorderOffset { get; }
        
        float AsteroidsSpawnTime { get; }
        float AsteroidsVelocityFrom { get; }
        float AsteroidsVelocityTo { get; }
        float AsteroidsRotationFrom { get; }
        float AsteroidsRotationTo { get; }
        
        float AliensVelocity { get; }
        float AliensFrameMaxRotation { get; }
        float AliensFireSpread { get; }
        float AliensFireTimelapse { get; }
        float AliensSpawnTimelapse { get; }
        float AliensAvoidDistance { get; }
        
        float BulletSpeed { get; }
        float BulletSpawnTime { get; }
        
        float SpaceshipOffset { get; }
        float BulletOffset { get; }
        float AlienOffset { get; }
        float LaserOffset { get; }
        
        int AlienHitScore { get; }
        int AsteroidHitScore { get; }
    }
}
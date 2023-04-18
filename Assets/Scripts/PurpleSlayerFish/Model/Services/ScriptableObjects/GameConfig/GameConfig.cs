using UnityEngine;

namespace PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig
{
    [CreateAssetMenu(menuName = "ScriptableObjects/GameConfig", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject, IGameConfig
    {
        [Header("Spaceship Config")]
        [SerializeField] private float _movementTime = 0.55f;
        public float MovementTime => _movementTime;
        [SerializeField] private float _rotationTime = 0.75f;
        public float RotationTime => _rotationTime;
        [SerializeField] private float movementVelocity = 3.5f;
        public float MovementVelocity => movementVelocity;
        [SerializeField] private float _rotationVelocity = 270f;
        public float RotationVelocity => _rotationVelocity;
        [SerializeField] private float _laserLifeTime = 0.15f;
        public float LaserLifeTime => _laserLifeTime;
        [SerializeField] private float _laserUseCooldown = 0.3f;
        public float LaserUseCooldown => _laserUseCooldown;
        [SerializeField] private float _laserDelay = 0.15f;
        public float LaserDelay => _laserDelay;
        [SerializeField] private int _laserMaxCount = 3;
        public int LaserMaxCount => _laserMaxCount;
        [SerializeField] private float _laserRestorationTime = 15f;
        public float LaserRestorationTime => _laserRestorationTime;
        
        [Header("Level Config")]
        [SerializeField] private float outerOuterBorderOffset = 2f;
        public float OuterBorderOffset => outerOuterBorderOffset;
        [SerializeField] private float _innerBorderOffset = 2f;
        public float InnerBorderOffset => _innerBorderOffset;
        
        [Header("Asteroids Config")]
        [SerializeField] private float _asteroidsSpawnTime = 1f;
        public float AsteroidsSpawnTime => _asteroidsSpawnTime;
        [SerializeField] private float _asteroidsVelocityFrom = 0.75f;
        public float AsteroidsVelocityFrom => _asteroidsVelocityFrom;
        [SerializeField] private float _asteroidsVelocityTo = 4f;
        public float AsteroidsVelocityTo => _asteroidsVelocityTo;
        [SerializeField] private float _asteroidsRotationFrom = 20f;
        public float AsteroidsRotationFrom => _asteroidsRotationFrom;
        [SerializeField] private float _asteroidsRotationTo = 120f;
        public float AsteroidsRotationTo => _asteroidsRotationTo;
        
        [Header("Aliens Config")]
        [SerializeField] private float _aliensVelocity = 1.7f;
        public float AliensVelocity => _aliensVelocity;
        [SerializeField] private float _aliensFrameMaxRotation = 1.3f;
        public float AliensFrameMaxRotation => _aliensFrameMaxRotation;
        [SerializeField] private float _aliensFireSpread = 17f;
        public float AliensFireSpread => _aliensFireSpread;
        [SerializeField] private float _aliensFireTimelapse = 3f;
        public float AliensFireTimelapse => _aliensFireTimelapse;
        [SerializeField] private float _aliensSpawnTime = 15f;
        public float AliensSpawnTimelapse => _aliensSpawnTime;
        [SerializeField] private float _aliensAvoidDistance = 4.5f;
        public float AliensAvoidDistance => _aliensAvoidDistance;
        
        [Header("Bullets Config")]
        [SerializeField] private float _bulletVelocity = 15f;
        public float BulletSpeed => _bulletVelocity;
        [SerializeField] private float _bulletSpawnTime = 0.17f;
        public float BulletSpawnTime => _bulletSpawnTime;

        [Header("Intersections Config")]
        [SerializeField] private float _spaceshipOffset = 0.55f;
        public float SpaceshipOffset => _spaceshipOffset;
        [SerializeField] private float _alienOffset = 0.55f;
        public float AlienOffset => _alienOffset;
        [SerializeField] private float _bulletOffset = 0.125f;
        public float BulletOffset => _bulletOffset;
        [SerializeField] private float _laserOffset;
        public float LaserOffset => _laserOffset;
        
        [Header("Score Config")]
        [SerializeField] private int _alienHitScore = 30;
        public int AlienHitScore => _alienHitScore;
        [SerializeField] private int _asteroidHitScore = 7;
        public int AsteroidHitScore => _asteroidHitScore;
    }
}
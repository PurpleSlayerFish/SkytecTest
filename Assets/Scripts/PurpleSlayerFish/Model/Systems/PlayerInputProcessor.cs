using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Windows.GameWindow;
using PurpleSlayerFish.Model.Entities;
using UnityEngine;
using Zenject;

namespace PurpleSlayerFish.Model.Systems
{
    public class PlayerInputProcessor : IRunSystem
    {
        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        private MathUtils _mathUtils = new();

        private PlayerEntity _entity;
        private float ROTATION_DAMPER = 0.01f;

        public void Run()
        {
            _entity = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            ProcessMovementInertia();
            ProcessRotationInertia();
            TryApplyMovement();
            TryFire();
        }

        private void ProcessMovementInertia()
        {
            _entity.MovementDuration = _mathUtils.Attenuation(_entity.MovementDuration, _mathUtils.Polarity(_entity.IsMovePerformed), _gameConfig.MovementTime);
            _entity.MovementMagnitude = _gameConfig.MovementVelocity * _entity.MovementDuration / _gameConfig.MovementTime;
            _subscriptionObserver.Execute(GameController.UPDATE_UI_VELOCITY, _entity.MovementMagnitude);
        }

        private void ProcessRotationInertia()
        {
            if (_entity.PerformedRotation == 0)
            {
                if (Mathf.Abs(_entity.RotationDuration) < ROTATION_DAMPER)
                    _entity.RotationDuration = 0;
                else
                    _entity.RotationDuration = _mathUtils.TwoSideAttenuation(_entity.RotationDuration, 
                        - Mathf.Sign(_entity.RotationDuration),_gameConfig.MovementTime);
            }
            else
                _entity.RotationDuration = _mathUtils.TwoSideAttenuation(_entity.RotationDuration, _entity.PerformedRotation,_gameConfig.MovementTime);
            _entity.WorldData.FrameRotation = - Mathf.Sign(_entity.RotationDuration) 
                * _gameConfig.RotationVelocity * Mathf.Abs(_entity.RotationDuration) / _gameConfig.RotationTime;
        }
        
        private void TryApplyMovement()
        {
            if (_entity.IsMovePerformed)
                _entity.LastRotation = _entity.WorldData.Rotation;
            _entity.WorldData.FrameMovement = _mathUtils.DirectionFromRotate(_entity.LastRotation) * _entity.MovementMagnitude;
        }
        
        private void TryFire()
        {
            if (!_entity.IsFirePerformed)
            {
                _entity.FireTimelapse = _gameConfig.BulletSpawnTime;
                return;
            }
            if (_entity.FireTimelapse >= _gameConfig.BulletSpawnTime)
                _subscriptionObserver.Execute(BulletProcessor.SUBSCRIPTION_PLAYER_FIRE);
            _entity.FireTimelapse -= Time.deltaTime;
            if (_entity.FireTimelapse < 0)
                _entity.FireTimelapse = _gameConfig.BulletSpawnTime;
        }
    }
}
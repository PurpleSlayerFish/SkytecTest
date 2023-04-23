using PurpleSlayerFish.Core.Presenter;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Systems;
using PurpleSlayerFish.Presenter.Utils;
using PurpleSlayerFish.View.Views;
using UnityEngine.InputSystem;

namespace PurpleSlayerFish.Presenter.Presenters
{
    public class PlayerPresenter : AbstractPresenter<PlayerEntity, PlayerView>
    {
        private ITransformSynchronizer _transformSynchronizer;
        
        public PlayerPresenter(PlayerEntity entity, PlayerView view, ISubscriptionObserver subscriptionObserver) : base(entity, view, subscriptionObserver)
        {
        }
        
        public override void ReloadEntity(PlayerEntity entity)
        {
            base.ReloadEntity(entity);
            _transformSynchronizer = new TransformSynchronizer2D(entity, _view.transform);
        }
        
        public override void SubscribeToEvents()
        {
            _view.PlayerInput.actions["Move"].performed += OnMovePerformed;
            _view.PlayerInput.actions["Rotate"].performed += OnRotate;
            _view.PlayerInput.actions["Fire"].performed += OnFirePerformed;
            _view.PlayerInput.actions["Laser"].performed += OnLaserPerformed;
            _view.PlayerInput.actions["Escape"].performed += OnEscapePerformed;
            _view.PlayerInput.actions["Move"].canceled += OnMoveCanceled;
            _view.PlayerInput.actions["Rotate"].canceled += OnRotate;
            _view.PlayerInput.actions["Fire"].canceled += OnFireCanceled;
            _subscriptionObserver.Subscribe(LaserProcessor.SUBSCRIPTION_LASER_PREPARE, ShowLaserFlash);
            _subscriptionObserver.Subscribe(LaserProcessor.SUBSCRIPTION_LASER_LAUNCHED, ShowLaser);
            _subscriptionObserver.Subscribe(LaserProcessor.SUBSCRIPTION_LASER_CANCELED, HideLaser);
            _subscriptionObserver.Subscribe(PlayerInstaller.SUBSCRIPTION_ON_PLAYER_DEATH, OnDeath);
        }

        public override void UnsubscribeToEvents()
        {
            _view.PlayerInput.actions["Move"].performed -= OnMovePerformed;
            _view.PlayerInput.actions["Rotate"].performed -= OnRotate;
            _view.PlayerInput.actions["Fire"].performed -= OnFirePerformed;
            _view.PlayerInput.actions["Laser"].performed -= OnLaserPerformed;
            _view.PlayerInput.actions["Escape"].performed -= OnEscapePerformed;
            _view.PlayerInput.actions["Move"].canceled -= OnMoveCanceled;
            _view.PlayerInput.actions["Rotate"].canceled -= OnRotate;
            _view.PlayerInput.actions["Fire"].canceled -= OnFireCanceled;
            _subscriptionObserver.Unsubscribe(LaserProcessor.SUBSCRIPTION_LASER_PREPARE);
            _subscriptionObserver.Unsubscribe(LaserProcessor.SUBSCRIPTION_LASER_LAUNCHED);
            _subscriptionObserver.Unsubscribe(LaserProcessor.SUBSCRIPTION_LASER_CANCELED);
            _subscriptionObserver.Unsubscribe(PlayerInstaller.SUBSCRIPTION_ON_PLAYER_DEATH);
        }

        private void OnMovePerformed(InputAction.CallbackContext _)
        {
            _entity.IsMovePerformed = true;
            SetTrailsActive(true);
        }
        private void OnMoveCanceled(InputAction.CallbackContext _)
        {
            _entity.IsMovePerformed = false;
            SetTrailsActive(false);
        }
        private void OnRotate(InputAction.CallbackContext context) => _entity.PerformedRotation = context.ReadValue<float>();
        private void OnFirePerformed(InputAction.CallbackContext _) => _entity.IsFirePerformed = true;
        private void OnFireCanceled(InputAction.CallbackContext _) => _entity.IsFirePerformed = false;
        private void OnLaserPerformed(InputAction.CallbackContext _) => _subscriptionObserver.Execute(LaserProcessor.SUBSCRIPTION_LASER_PERFORMED);
        private void OnEscapePerformed(InputAction.CallbackContext _) => _subscriptionObserver.Execute(PauseInstaller.SUBSCRIPTION_PAUSE);

        private void SetTrailsActive(bool value)
        {
            for (int i = 0; i < _view.Trails.Length; i++)
                _view.Trails[i].SetActive(value);
        }

        private void ShowLaserFlash() => _view.LaserFlash.SetActive(true);
        private void ShowLaser()
        {
            _view.LaserFlash.SetActive(false);
            _view.Laser.SetActive(true);
        }
        private void HideLaser() => _view.Laser.SetActive(false);

        private void OnDeath() => _view.gameObject.SetActive(false);
        
        public override void Dispose()
        {
            base.Dispose();
            _transformSynchronizer.Dispose();
            _transformSynchronizer = null;
        }
    }
}
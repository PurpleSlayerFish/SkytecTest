using System;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Windows.Container;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using PurpleSlayerFish.Model.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;
using PurpleSlayerFish.Windows.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace PurpleSlayerFish.Model.Systems
{
    public class PlayerInstaller : IInstallSystem
    {
        public const string SUBSCRIPTION_ON_PLAYER_HIT = "on_player_hit";
        public const string SUBSCRIPTION_ON_PLAYER_DEATH = "on_player_death";
        private const string PLAYER_PREFAB_BUNDLE = "prefabs";
        private const string PLAYER_PREFAB_NAME = "Player";

        private IEntitiesContext _entitiesContext;
        private IPrefabProvider _prefabProvider;
        private ISubscriptionObserver _subscriptionObserver;
        private IGameConfig _gameConfig;
        private IUiContainer _uiContainer;
        
        private PlayerEntity _player;

        public PlayerInstaller(IEntitiesContext entitiesContext, IPrefabProvider prefabProvider, 
            ISubscriptionObserver subscriptionObserver, IGameConfig gameConfig, IUiContainer uiContainer)
        {
            _entitiesContext = entitiesContext;
            _prefabProvider = prefabProvider;
            _subscriptionObserver = subscriptionObserver;
            _gameConfig = gameConfig;
            _uiContainer = uiContainer;
        }

        public void Install()
        {
            _player = new PlayerEntity();
            var view = Object.Instantiate(_prefabProvider.Get<GameObject>(PLAYER_PREFAB_BUNDLE, PLAYER_PREFAB_NAME))
                .GetComponent<PlayerView>();
            if (view == null)
                throw new NullReferenceException("GameObject has no PlayerView component!");

            _player.LaserUsesCount = 1;
            _player.WorldData.IntersectionOffset = _gameConfig.SpaceshipOffset;
            _player.WorldData.OnPositionChange += () => _subscriptionObserver.Execute(GameController.UPDATE_UI_POSITION, _player.WorldData.Position);
            _player.WorldData.OnRotationChange += () => _subscriptionObserver.Execute(GameController.UPDATE_UI_ROTATION, _player.WorldData.Rotation);
            _player.IsAlive = true;
            
            _entitiesContext.Insert(PlayerEntity.ENTITY_TYPE, _player);
            _entitiesContext.Insert(DynamicTransformProcessor.DYNAMIC_TRANSFORM_ENTITY_TYPE, _player);
            new PlayerPresenter(_player, view, _subscriptionObserver);
            
            _subscriptionObserver.Subscribe(SUBSCRIPTION_ON_PLAYER_HIT, OnPlayerHit);
        }

        private void OnPlayerHit()
        {
            _player = _entitiesContext.SelectFirst<PlayerEntity>(PlayerEntity.ENTITY_TYPE);
            _player.IsAlive = false;
            _subscriptionObserver.Execute(SUBSCRIPTION_ON_PLAYER_DEATH);
            _uiContainer.Hide<GameController>();
            _uiContainer.BuildDialog()
                .WithLabel("Game over")
                .WithDescription("Score: " + _player.Score)
                .WithButton("Restart", () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex))
                .Build()
                .Show();
        }
    }
}
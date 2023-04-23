using System;
using PurpleSlayerFish.Core.Data;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Model.Systems;
using PurpleSlayerFish.Core.Services.DataStorage;
using PurpleSlayerFish.Core.Services.SceneLoader;
using PurpleSlayerFish.Core.Services.ScriptableObjects.GameConfig;
using PurpleSlayerFish.Core.Services.SubscriptionObserver;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.Windows.GameWindow;
using PurpleSlayerFish.Model.Entities;
using PurpleSlayerFish.Presenter.Presenters;
using PurpleSlayerFish.View.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using IPrefabProvider = PurpleSlayerFish.Core.Services.PrefabProvider.IPrefabProvider;
using Object = UnityEngine.Object;

namespace PurpleSlayerFish.Model.Systems
{
    public class PlayerInstaller : IInstallSystem
    {
        public const string SUBSCRIPTION_ON_PLAYER_HIT = "on_player_hit";
        public const string SUBSCRIPTION_ON_PLAYER_DEATH = "on_player_death";
        private const string PLAYER_PREFAB_BUNDLE = "prefabs";
        private const string PLAYER_PREFAB_NAME = "Player";

        [Inject] private IEntitiesContext _entitiesContext;
        [Inject] private IPrefabProvider _prefabProvider;
        [Inject] private ISubscriptionObserver _subscriptionObserver;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private IUiContainer _uiContainer;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IDataStorage<PlayerData> _dataStorage;
        
        private PlayerEntity _player;

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
                .WithButton("Restart", () => _sceneLoader.Load(SceneManager.GetActiveScene().name))
                .Build()
                .Show();

            if (_dataStorage.Load().Score < _player.Score)
                _dataStorage.Save(new PlayerData{Score = _player.Score});
        }
    }
}
using System.Collections.Generic;
using PurpleSlayerFish.Core.Model;
using PurpleSlayerFish.Core.Windows.Dialogs;
using PurpleSlayerFish.Core.Windows.ElementManager;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using PurpleSlayerFish.Model.Services.SubscriptionObserver;
using UnityEngine;

namespace PurpleSlayerFish.Core.Windows.Container
{
    public class UiContainer : IUiContainer
    {
        // todo переписать вызов экрана на сигнал бас
        private IPrefabProvider _prefabProvider;
        private ISubscriptionObserver _subscriptionObserver;
        private Dictionary<string, AbstractController> _uiControllers;
        private UiProvider _uiProvider;

        private DialogWindow _dialogPrefab;

        public UiContainer(IPrefabProvider prefabProvider, ISubscriptionObserver subscriptionObserver)
        {
            _prefabProvider = prefabProvider;
            _subscriptionObserver = subscriptionObserver;
            _uiProvider = Object.Instantiate(_prefabProvider.Get<GameObject>(GameProcessor.CORE_BUNDLE, GameProcessor.UI_PREFAB)).GetComponent<UiProvider>();
            InitializeWindows();
        }

        private void InitializeWindows()
        {
            AbstractController controller;
            AbstractWindow windowPrefab;
            _uiControllers = new Dictionary<string, AbstractController>();
            var windows = _prefabProvider.Get(GameProcessor.UI_WINDOWS_BUNDLE);
            for (int i = 0; i < windows.Length; i++)
            {
                windowPrefab = (windows[i] as GameObject).GetComponent<AbstractWindow>();
                if (windowPrefab is DialogWindow window)
                    _dialogPrefab = window;
                else
                {
                    controller = Object.Instantiate(windowPrefab, _uiProvider.RootCanvas.transform).Initialize(_subscriptionObserver);
                    _uiControllers.Add(controller.GetType().Name, controller);
                }
            }
        }

        public T Get<T>() where T : AbstractController => _uiControllers[typeof(T).Name] as T;
        public void Show<T>() where T : AbstractController => _uiControllers[typeof(T).Name].Show();
        public void Hide<T>() where T : AbstractController => _uiControllers[typeof(T).Name].Hide();
        public DialogBuilder BuildDialog() => 
            new(Object.Instantiate(_dialogPrefab, _uiProvider.RootCanvas.transform), 
                new UiElementManager(_prefabProvider), _subscriptionObserver);
    }
}
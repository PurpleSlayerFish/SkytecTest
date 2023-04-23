using System.Collections.Generic;
using PurpleSlayerFish.Core.Global;
using PurpleSlayerFish.Core.Ui.Dialogs;
using PurpleSlayerFish.Core.Ui.ElementManager;
using PurpleSlayerFish.Core.Ui.Windows;
using UnityEngine;
using Zenject;
using IPrefabProvider = PurpleSlayerFish.Core.Services.PrefabProvider.IPrefabProvider;

namespace PurpleSlayerFish.Core.Ui.Container
{
    public class UiContainer : IUiContainer
    {
        // todo переписать вызов экрана на сигнал бас
        [Inject] private IPrefabProvider _prefabProvider;
        [Inject] private IUiElementManager _uiElementManager;
        private Dictionary<string, AbstractController> _uiControllers;
        private List<string> _inRoot;
        private UiProvider _uiProvider;
        private DiContainer _container;

        private DialogWindow _dialogPrefab;

        private string _cashedName;

        public void Install(DiContainer container)
        {
            _container = container;
            _uiControllers = new Dictionary<string, AbstractController>();
            _inRoot = new List<string>();
            _uiProvider = _prefabProvider.Instantiate<UiProvider>(UiGlobal.CORE_BUNDLE, UiGlobal.ROOT_PREFAB);
            _dialogPrefab = _prefabProvider.GetComponent<DialogWindow>(UiGlobal.WINDOWS_BUNDLE, nameof(DialogWindow));
            Object.DontDestroyOnLoad(_uiProvider);
        }

        public void InitializeWindow<T>(bool inRoot = true) where T : AbstractWindow
        {
            AddToDictionary(_prefabProvider.Instantiate<AbstractWindow>(UiGlobal.WINDOWS_BUNDLE, typeof(T).Name,
                inRoot ? _uiProvider.RootCanvas.transform : _uiProvider.transform).Initialize(_container), inRoot);
        }

        public void ClearRoot()
        {
            Transform child;
            for (int i =  _uiProvider.RootCanvas.transform.childCount - 1; i >= 0; i--)
            {
                child =  _uiProvider.RootCanvas.transform.GetChild(i);
                child.SetParent(null);
                Object.Destroy(child.gameObject);
            }

            for (int i = 0; i < _inRoot.Count; i++)
                _uiControllers.Remove(_inRoot[i]);
        }

        public T Get<T>() where T : AbstractController => _uiControllers[typeof(T).Name] as T;
        public void Show<T>() where T : AbstractController => _uiControllers[typeof(T).Name].Show();
        public void Hide<T>() where T : AbstractController => _uiControllers[typeof(T).Name].Hide();
        public DialogBuilder BuildDialog() => 
            new(_prefabProvider.Instantiate<DialogWindow>(_dialogPrefab.gameObject, _uiProvider.RootCanvas.transform), _uiElementManager, _container);

        private void AddToDictionary(AbstractController controller, bool inRoot)
        {
            _cashedName = controller.GetType().Name;
            _uiControllers.Add(_cashedName, controller);
            if (inRoot)
                _inRoot.Add(_cashedName);
        }
    }
}
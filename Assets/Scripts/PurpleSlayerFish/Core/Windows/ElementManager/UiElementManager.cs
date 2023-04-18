using PurpleSlayerFish.Core.Windows.ElementManager.Elements;
using PurpleSlayerFish.Model.Services.PrefabProvider;
using UnityEngine;

namespace PurpleSlayerFish.Core.Windows.ElementManager
{
    public class UiElementManager : IUiElementManager
    {
        private const string UI_ELEMENTS_BUNDLE = "ui_elements";
        private const string BUTTON_PREFAB = "Button";
        private IPrefabProvider _prefabProvider;
        
        public UiElementManager(IPrefabProvider prefabProvider)
        {
            _prefabProvider = prefabProvider;
        }

        public ExtendedButtonBuilder BuildButton() => new(Object.Instantiate(_prefabProvider.GetComponent<ExtendedButton>(UI_ELEMENTS_BUNDLE, BUTTON_PREFAB)));
    }
}
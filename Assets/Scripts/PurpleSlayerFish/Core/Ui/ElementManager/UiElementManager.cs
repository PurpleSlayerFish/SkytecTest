using PurpleSlayerFish.Core.Global;
using PurpleSlayerFish.Core.Ui.ElementManager.Elements;
using UnityEngine;
using Zenject;
using IPrefabProvider = PurpleSlayerFish.Core.Services.PrefabProvider.IPrefabProvider;

namespace PurpleSlayerFish.Core.Ui.ElementManager
{
    public class UiElementManager : IUiElementManager
    {
        [Inject] private IPrefabProvider _prefabProvider;
        
        public ButtonBuilder BuildButton() => new(Object.Instantiate(
            _prefabProvider.GetComponent<ExtendedButton>(UiGlobal.ELEMENTS_BUNDLE, UiGlobal.BUTTON_PREFAB)));
        public PurchaseBoxBuilder BuildPurchaseBox(bool isBig) => new(Object.Instantiate(
            _prefabProvider.GetComponent<PurchaseBox>(UiGlobal.ELEMENTS_BUNDLE, isBig ? UiGlobal.PURCHASE_BOX_BIG_PREFAB : UiGlobal.PURCHASE_BOX_SMALL_PREFAB)));
    }
}
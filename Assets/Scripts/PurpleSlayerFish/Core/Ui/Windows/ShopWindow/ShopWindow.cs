using PurpleSlayerFish.Core.Ui.ElementManager.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace PurpleSlayerFish.Core.Ui.Windows.ShopWindow
{
    public class ShopWindow : AbstractWindow<ShopController>
    {
        [Header("Purchase Boxes")] 
        [SerializeField] private LayoutGroup _purchasesGroup;
        public LayoutGroup PurchasesGroup => _purchasesGroup;
        [Header("Exit")] 
        [SerializeField] private ExtendedButton _exitButton;
        public ExtendedButton ExitButton => _exitButton;
    }
}
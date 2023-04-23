using System.Text;
using PurpleSlayerFish.Core.Data;
using PurpleSlayerFish.Core.Services.Purchases;
using PurpleSlayerFish.Core.Ui.Container;
using PurpleSlayerFish.Core.Ui.ElementManager;
using Zenject;

namespace PurpleSlayerFish.Core.Ui.Windows.ShopWindow
{
    public class ShopController : AbstractController<ShopWindow>
    {
        [Inject] private IUiContainer _uiContainer;
        [Inject] private IUiElementManager _uiElementManager;
        [Inject] private IPurchaseService _purchaseService;

        protected override void AfterInitialize()
        {
            InitializePurchases();
            _window.ExitButton.AddOnClick(Hide);
        }

        private void InitializePurchases()
        {
            var purchases = _purchaseService.GetPurchases();
            for (int i = 0; i < purchases.ShopItems.Length; i++)
            {
                var item = purchases.ShopItems[i];
                _uiElementManager.BuildPurchaseBox(item.Items != null)
                    .WithLabel(GetLabel(item))
                    .WithPrice($"{item.Price} {item.Currency}")
                    .WithAction(() => TryPurchase(item))
                    .Build().transform.SetParent(_window.PurchasesGroup.transform);
            }
        }

        private string GetLabel(ShopItem item)
        {
            var label = item.Amount == 0 ? item.Key : $"{item.Key} x{item.Amount}";
            if(item.Amount == 0)
            {
                var builder = new StringBuilder(label);
                for (int i = 0; i < item.Items.Length; i++)
                {
                    builder.Append("\n").Append(item.Items[i].Key);
                    if (item.Items[i].Damage != null)
                        builder.Append(": ").Append("damage - ").Append(item.Items[i].Key);
                }
                label = builder.ToString();
            }

            return label;
        }

        private void TryPurchase(ShopItem item)
        {
            if (_purchaseService.ApplyPurchase(item.Key))
            {
                _uiContainer.BuildDialog()
                    .WithLabel("Complete!")
                    .WithDescription($"You purchase {item.Key}!")
                    .WithButton("Ok!", () => SetInteractable(true), true)
                    .Build()
                    .Show();
                
                SetInteractable(false);
            }
        }
    }
}
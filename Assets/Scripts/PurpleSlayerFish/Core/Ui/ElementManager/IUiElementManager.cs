using PurpleSlayerFish.Core.Ui.ElementManager.Elements;

namespace PurpleSlayerFish.Core.Ui.ElementManager
{
    public interface IUiElementManager
    {
        ButtonBuilder BuildButton();
        PurchaseBoxBuilder BuildPurchaseBox(bool isBig = false);
    }
}
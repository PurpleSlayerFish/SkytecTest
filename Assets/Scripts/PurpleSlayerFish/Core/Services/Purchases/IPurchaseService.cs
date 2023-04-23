using PurpleSlayerFish.Core.Data;

namespace PurpleSlayerFish.Core.Services.Purchases
{
    public interface IPurchaseService
    {
        ShopData GetPurchases();
        
        bool ApplyPurchase(string key);
    }
}
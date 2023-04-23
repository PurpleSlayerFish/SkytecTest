using Newtonsoft.Json;
using PurpleSlayerFish.Core.Data;

namespace PurpleSlayerFish.Core.Services.Purchases
{
    public class TestPurchaseService : IPurchaseService
    {
        private const string PURCHASES_JSON =  
        "{ shopItems:[{ key: \"gold\", amount: 100, price: \"1.99\", currency: \"usd\" },"+
        "{ key: \"silver\", amount: 5000, price: \"0.99\", currency: \"usd\" },"+
        "{ key: \"starterPack\", items: [{ key: \"sword\", damage: \"14\" }, { key: \"ressurect\", amount: 3 }], price: \"1.99\", currency: \"usd\" },]}";
        
        public ShopData GetPurchases() => JsonConvert.DeserializeObject<ShopData>(PURCHASES_JSON);

        public bool ApplyPurchase(string key) => true;
    }
}
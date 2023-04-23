using System;

namespace PurpleSlayerFish.Core.Data
{
    [Serializable]
    public struct ShopData
    {
        public ShopItem[] ShopItems;
    }
    
    [Serializable]
    public struct ShopItem
    {
        public string Key;
        public int Amount;
        public string Price;
        public string Currency;
        public Item[] Items;
    }

    [Serializable]
    public struct Item
    {
        public string Key;
        public string Damage;
    }
}
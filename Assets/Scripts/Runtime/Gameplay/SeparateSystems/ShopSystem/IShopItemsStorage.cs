using System.Collections.Generic;

namespace Runtime.Gameplay.SeparateSystems.ShopSystem
{
    public interface IShopItemsStorage
    {
        void AddItemDisplay(ShopItemDisplayView shopItemDisplay);
        
        public void SortItemsByNotPurchased();

        List<ShopItemDisplayView> GetItemDisplay();

        void SetShopStateConfigs(List<ShopItemStateConfig> shopConfigShopItemStateConfigs);

        ShopItemStateConfig GetItemStateConfig(ShopItemState shopItemState);

        void Cleanup();
    }
}
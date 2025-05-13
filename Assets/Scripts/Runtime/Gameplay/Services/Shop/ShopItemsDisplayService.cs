using Runtime.Gameplay.SeparateSystems.ShopSystem;

namespace Runtime.Gameplay.Services.Shop
{
    public class ShopItemsDisplayService : IShopItemsDisplayService
    {
        private readonly ShopItemDisplayController _shopItemDisplayController;

        private ShopSetup _shopSetup;

        public ShopItemsDisplayService(ShopItemDisplayController shopItemDisplayController) =>
                _shopItemDisplayController = shopItemDisplayController;

        public void SetShopSetup(ShopSetup shopSetup) =>
                _shopSetup = shopSetup;

        public void CreateShopItems()
        {
            foreach (var shopItem in _shopSetup.ShopItems)
                _shopItemDisplayController.CreateItemDisplayView(shopItem);
            
            _shopItemDisplayController.SortItemsByNotPurchased();
        }

        public void UpdateItemsStatus() =>
                _shopItemDisplayController.UpdateItemStates();
    }
}
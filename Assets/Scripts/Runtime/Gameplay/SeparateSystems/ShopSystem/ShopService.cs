using Runtime.Gameplay.Services.Shop;
using Runtime.Gameplay.Services.UserData.Data;

namespace Runtime.Gameplay.SeparateSystems.ShopSystem
{
    public class ShopService : ISetShopSetup
    {
        private readonly IUserInventoryService _userInventoryService;

        private ShopSetup _shopSetup;

        public ShopService(IUserInventoryService userInventoryService) =>
                _userInventoryService = userInventoryService;

        public void SetShopSetup(ShopSetup shopSetup) =>
                _shopSetup = shopSetup;

        public void ProcessPurchase(ShopItemDisplayView shopItemDisplayView)
        {
            _userInventoryService.AddPurchasedGameItemID(shopItemDisplayView.GetShopItemModel().ShopItem.ItemID);
            _userInventoryService.AddBalance(-shopItemDisplayView.GetShopItemModel().ShopItem.ItemPrice);
        }

        public bool EnoughBalance(ShopItem shopItem) => _userInventoryService.GetBalance() >= shopItem.ItemPrice;

        public bool IsItemPurchased(ShopItem shopItem) => _userInventoryService.GetPurchasedGameItemsIDs().Contains(shopItem.ItemID);
    }
}
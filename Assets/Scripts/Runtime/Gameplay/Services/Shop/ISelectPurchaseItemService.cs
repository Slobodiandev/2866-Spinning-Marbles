using Runtime.Gameplay.SeparateSystems.ShopSystem;

namespace Runtime.Gameplay.Services.Shop
{
    public interface ISelectPurchaseItemService : ISetShopSetup
    {
        void SelectPurchasedItem(ShopItemDisplayModel shopItemModel);
    }
}
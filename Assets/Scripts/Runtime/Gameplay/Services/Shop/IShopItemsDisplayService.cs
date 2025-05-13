
namespace Runtime.Gameplay.Services.Shop
{
    public interface IShopItemsDisplayService : ISetShopSetup
    {
        void CreateShopItems();

        void UpdateItemsStatus();
    }
}
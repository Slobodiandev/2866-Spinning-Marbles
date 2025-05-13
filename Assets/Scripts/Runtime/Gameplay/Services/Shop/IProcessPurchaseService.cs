using System.Threading;
using Runtime.Gameplay.SeparateSystems.ShopSystem;

namespace Runtime.Gameplay.Services.Shop
{
    public interface IProcessPurchaseService : ISetShopSetup
    {
        void ProcessPurchaseAttempt(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken);
    }
}
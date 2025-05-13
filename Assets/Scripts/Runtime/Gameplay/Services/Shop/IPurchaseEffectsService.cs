using System.Threading;
using Runtime.Gameplay.SeparateSystems.ShopSystem;

namespace Runtime.Gameplay.Services.Shop
{
    public interface IPurchaseEffectsService : ISetShopSetup
    {
        void PlayFailedPurchaseAttemptEffect(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken);
    }
}
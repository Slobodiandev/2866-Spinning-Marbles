using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Runtime.Gameplay.Services.IAP
{
    public class IAPServiceMock : IIAPService
    {
        private readonly ICustomLogger _customLogger;
        public IAPServiceMock(ICustomLogger customLogger)
        {
            _customLogger = customLogger;
        }

        void IIAPService.Initialize(List<ProductData> products)
        {
            _customLogger.Log($"{nameof(IAPServiceMock)}: initialized!");
        }

        Product IIAPService.GetProductById(string productId)
        {
            return null;
        }

        async UniTask<bool> IIAPService.TryBuyProduct(string productId)
        {
            _customLogger.Log($"{nameof(IAPServiceMock)}: Try buy product {productId}");

            await UniTask.Delay(2000);
            return true;
        }

        bool IIAPService.IsProductPurchased(string productId)
        {
            return false;
        }

        bool IIAPService.IsInitialized()
        {
            return true;
        }

        public string GetProductPrice(string productId)
        {
            return "UAH 123";
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
        }
    }
}
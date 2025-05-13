using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Runtime.Gameplay.Services.IAP
{
    public class IAPService : IIAPService, IDetailedStoreListener
    {
        private readonly ICustomLogger _customLogger;

        private IStoreController _storeController;
        private IExtensionProvider _storeExtensionProvider;

        private UniTaskCompletionSource<bool> _purchaseCompletionSource;
        private ConfigurationBuilder _builder;

        public IAPService(ICustomLogger customLogger)
        {
            _customLogger = customLogger;
        }

        void IIAPService.Initialize(List<ProductData> products)
        {
            _customLogger.Log($"{nameof(IAPService)}: initialization started");

            _builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            AddProductsToBuilder(_builder, products);
            InitializeUnityPurchasing(_builder);
        }

        Product IIAPService.GetProductById(string productId)
        {
            if (IsInitialized())
                return _storeController.products.WithID(productId);

            _customLogger.Warning($"{nameof(IAPService)} is not initialized yet.");
            return null;
        }

        async UniTask<bool> IIAPService.TryBuyProduct(string productId)
        {
            _customLogger.Log($"{nameof(IAPService)}: Try buy product {productId}");

            if (IsInitialized())
            {
                _purchaseCompletionSource = new UniTaskCompletionSource<bool>();
                _storeController.InitiatePurchase(productId);
                bool purchaseCompleteSuccessfully = await _purchaseCompletionSource.Task;

                return purchaseCompleteSuccessfully;
            }

            return false;
        }

        bool IIAPService.IsProductPurchased(string productId)
        {
            Product product = CodelessIAPStoreListener.Instance.GetProduct(productId);

            if (product is { hasReceipt: true })
            {
                _customLogger.Log($"{nameof(IAPService)}: {productId} : has been bought");
                return true;
            }

            _customLogger.Log(product == null
                ? $"{nameof(IAPService)}: {productId} : no product found"
                : $"{nameof(IAPService)}: {productId} : has no receipt");

            return false;
        }

        bool IIAPService.IsInitialized()
        {
            return IsInitialized();
        }

        string IIAPService.GetProductPrice(string productId)
        {
            if (IsInitialized())
            {
                var product = _storeController.products.WithID(productId);
                return product.metadata.localizedPriceString;
            }
            _customLogger.Warning($"{nameof(IAPService)}, failed get price, service not initialized yet.");
            return string.Empty;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _customLogger.Log($"{nameof(IAPService)}: Init complete");

            _storeController = controller;
            _storeExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _customLogger.Error($"{nameof(IAPService)}: OnInitializeFailed: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            _customLogger.Error($"{nameof(IAPService)}: OnInitializeFailed: [{message}] - {error}");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            _customLogger.Log(
                $"{nameof(IAPService)}: ProcessPurchase: {purchaseEvent?.purchasedProduct?.definition?.id ?? "Unknown Id"} for {(purchaseEvent?.purchasedProduct?.metadata?.localizedPriceString ?? "Unknown Price")}");

            _purchaseCompletionSource?.TrySetResult(true);

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            _purchaseCompletionSource?.TrySetResult(false);

            _customLogger.Error(
                $"{nameof(IAPService)}: Purchase failed {failureDescription.productId}: {failureDescription.message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            _purchaseCompletionSource?.TrySetResult(false);

            _customLogger.Error($"{nameof(IAPService)}: OnPurchaseFailed: {failureReason}");
        }

        private bool IsInitialized()
        {
            return _storeController != null && _storeExtensionProvider != null;
        }

        private void AddProductsToBuilder(ConfigurationBuilder builder, List<ProductData> products)
        {
            foreach (ProductData product in products)
            {
                _customLogger.Log($"{nameof(IAPService)}: {product.ProductId} added as product");
                builder.AddProduct(product.ProductId, product.ProductType);
            }
        }

        private void InitializeUnityPurchasing(ConfigurationBuilder builder)
        {
            UnityPurchasing.Initialize(this, builder);
        }
    }
}
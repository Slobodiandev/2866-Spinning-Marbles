using System.Threading;
using Core.Services.Audio;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.Services.Audio;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Popup;
using Runtime.Gameplay.UI.Popup.Data;

namespace Runtime.Gameplay.Services.Shop
{
    public class ProcessPurchaseService : IProcessPurchaseService
    {
        private readonly ShopService _shopService;
        private readonly IPurchaseEffectsService _purchaseEffectsService;
        private readonly ISelectPurchaseItemService _selectPurchaseItemService;
        private readonly IUiService _uiService;
        private readonly IShopItemsDisplayService _shopItemsDisplayService;
        private readonly IAudioService _audioService;
        
        private ShopSetup _shopSetup;

        public ProcessPurchaseService(ShopService shopService, IPurchaseEffectsService purchaseEffectsService, 
                ISelectPurchaseItemService selectPurchaseItemService, IUiService uiService, 
                IShopItemsDisplayService shopItemsDisplayService, IAudioService audioService)
        {
            _shopService = shopService;
            _purchaseEffectsService = purchaseEffectsService;
            _selectPurchaseItemService = selectPurchaseItemService;
            _uiService = uiService;
            _shopItemsDisplayService = shopItemsDisplayService;
            _audioService = audioService;
        }

        public void SetShopSetup(ShopSetup shopSetup) =>
                _shopSetup = shopSetup;

        public void ProcessPurchaseAttempt(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
        {
            var shopItemModel = shopItemDisplayView.GetShopItemModel();
            
            switch (shopItemModel.ItemState)
            {
                case ShopItemState.NotPurchased:
                    ProcessPurchase(shopItemDisplayView, cancellationToken);
                    break;
                case ShopItemState.Purchased:
                    SelectItem(shopItemModel);
                    UpdateStatus();
                    break;
                case ShopItemState.Selected:
                    UpdateStatus();
                    break;
            }
        }

        private async void ProcessPurchase(ShopItemDisplayView shopItemDisplayView, CancellationToken cancellationToken)
        {
            if(!_shopService.EnoughBalance(shopItemDisplayView.GetShopItemModel().ShopItem))
            {
                _purchaseEffectsService.PlayFailedPurchaseAttemptEffect(shopItemDisplayView, cancellationToken);
                return;
            }

            if (_shopSetup.ConfirmPurchase)
            {
                var popup = await _uiService
                    .ShowWindow(ConstPopups.ItemPurchasePopup,
                        new ItemPurchaseWindowData { ShopItem = shopItemDisplayView.GetShopItemModel().ShopItem },
                        cancellationToken) as ItemPurchaseWindow;

                Subscribe(shopItemDisplayView, popup);
            }
            else
                AcceptPurchase(shopItemDisplayView);
        }

        private void Subscribe(ShopItemDisplayView shopItemDisplayView, ItemPurchaseWindow window)
        {
            window.OnAcceptPressedEvent += () => { OnAcceptButtonPressed(shopItemDisplayView, window); };
            window.OnDenyPressedEvent += () => OnDenyButtonPressed(shopItemDisplayView, window);
        }

        private void SelectItem(ShopItemDisplayModel shopDisplayModel)
        {
            PlaySound(ConstAudio.SelectSound, _shopSetup.PurchaseEffectSettings.PlaySoundOnSelectPurchased);
            _selectPurchaseItemService.SelectPurchasedItem(shopDisplayModel);
        }

        private void PlaySound(string sound, bool condition)
        {
            if (condition)
                _audioService.PlaySound(sound);
        }

        private void UpdateStatus() =>
                _shopItemsDisplayService.UpdateItemsStatus();

        private static void DestroyPopup(ItemPurchaseWindow window) =>
                window.DestroyWindow();

        private void OnDenyButtonPressed(ShopItemDisplayView shopItemDisplayView, ItemPurchaseWindow window)
        {
            window.OnAcceptPressedEvent -= () => { OnAcceptButtonPressed(shopItemDisplayView, window); };
            DestroyPopup(window);
        }

        private void AcceptPurchase(ShopItemDisplayView shopItemDisplayView)
        {
            _shopService.ProcessPurchase(shopItemDisplayView);

            SelectItem(shopItemDisplayView.GetShopItemModel());
            PlaySound(ConstAudio.PurchaseSound, condition: _shopSetup.PurchaseEffectSettings.PlaySoundOnPurchase);
            UpdateStatus();
        }
        
        private void OnAcceptButtonPressed(ShopItemDisplayView shopItemDisplayView, ItemPurchaseWindow window)
        {
            AcceptPurchase(shopItemDisplayView);
            DestroyPopup(window);
        }
    }
}
using System;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.UI.Popup.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class ItemPurchaseWindow : BaseWindow
    {
        [SerializeField] private Image _shopItemImage;
        [SerializeField] private TextMeshProUGUI _shopItemPrice;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _denyButton;

        public event Action OnAcceptPressedEvent;
        public event Action OnDenyPressedEvent;

        private void OnDestroy()
        {
            _acceptButton.onClick.RemoveAllListeners();
            _denyButton.onClick.RemoveAllListeners();
        }

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            SetData(data as ItemPurchaseWindowData);
            SubscribeToEvents();
            return base.ShowWindow(data, cancellationToken);
        }

        private void SetData(ItemPurchaseWindowData data)
        {
            _shopItemImage.sprite = data.ShopItem.ItemSprite;
            _shopItemPrice.text = data.ShopItem.ItemPrice.ToString();
        }

        private void SubscribeToEvents()
        {
            _acceptButton.onClick.AddListener(() => OnAcceptPressedEvent?.Invoke());
            _denyButton.onClick.AddListener(() => OnDenyPressedEvent?.Invoke());
        }
    }
}
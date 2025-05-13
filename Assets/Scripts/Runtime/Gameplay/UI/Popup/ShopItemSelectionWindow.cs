using System;
using System.Collections.Generic;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class ShopItemSelectionWindow : BaseWindow
    {
        [SerializeField] private RectTransform _selectionParent;
        [SerializeField] private Image _selectedBallImage;
        [SerializeField] private Button _randomButton;
        [SerializeField] private Button _closeButton;
        
        public event Action OnRandomButtonPressed;
        public event Action OnClosePressed;
        
        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _randomButton.onClick.AddListener(() => OnRandomButtonPressed?.Invoke());
            _closeButton.onClick.AddListener(() => OnClosePressed?.Invoke());
            return base.ShowWindow(data, cancellationToken);
        }

        public void SetUsedSprite(Sprite usedSprite) => _selectedBallImage.sprite = usedSprite;
        
        public void ParentChoices(List<InventoryItemSelectionView> selectionViews)
        {
            foreach (var item in selectionViews)
            {
                item.transform.SetParent(_selectionParent, false);
            }
        }
    }
}
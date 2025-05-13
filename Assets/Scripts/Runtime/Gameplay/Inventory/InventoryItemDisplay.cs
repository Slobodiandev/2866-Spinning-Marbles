using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Inventory
{
    public class InventoryItemDisplay : MonoBehaviour
    {
        [SerializeField] private Button _selectButton;
        [SerializeField] private Image _selectedBallImage;

        private SelectedItemModel _selectedItemModel;
    
        public event Action<SelectedItemModel> OnSelectPressed;
    
        public void Initialize(SelectedItemModel inventoryItemModel)
        {
            _selectedItemModel = inventoryItemModel;
        
            _selectedBallImage.sprite = _selectedItemModel.ItemSprite;
            _selectButton.onClick.AddListener(() => OnSelectPressed?.Invoke(_selectedItemModel));
        
            inventoryItemModel.OnItemIdChanged += UpdateView;
        }

        private void UpdateView(int _, Sprite newSprite)
        {
            _selectedBallImage.sprite = newSprite;
        }
    }
}

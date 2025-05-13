using System;
using UnityEngine;

namespace Runtime.Gameplay.Inventory
{
    public class SelectedItemModel
    {
        private int _itemSlotID;
        private int _shopItemID;
        private Sprite _itemSprite;

        public int ItemSlotID => _itemSlotID;
        public int ShopItemID => _shopItemID;
        public Sprite ItemSprite => _itemSprite;

        public event Action<int, Sprite> OnItemIdChanged;
    
        public SelectedItemModel(int itemSlotID, int shopItemID, Sprite itemSprite)
        {
            _itemSlotID = itemSlotID;
            _shopItemID = shopItemID;
            _itemSprite = itemSprite;
        }

        public void UpdateShopItemID(int newShopItemID, Sprite newItemSprite)
        {
            _shopItemID = newShopItemID;
            _itemSprite = newItemSprite;
        
            OnItemIdChanged?.Invoke(_shopItemID, _itemSprite);
        }
    }
}

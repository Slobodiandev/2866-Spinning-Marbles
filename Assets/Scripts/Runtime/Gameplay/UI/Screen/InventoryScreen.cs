using System;
using System.Collections.Generic;
using Runtime.Gameplay.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Screen
{
    public class InventoryScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _parent;
        
        public event Action OnBackPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
        }

        public void ParentItems(List<InventoryItemDisplay> items)
        {
            foreach (var item in items)
            {
                item.transform.SetParent(_parent, false);
            }
        }
    }
}
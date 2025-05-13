using System.Collections.Generic;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData.Data;
using Runtime.Gameplay.UI.Popup;
using UnityEngine;

namespace Runtime.Gameplay.Inventory
{
    public class ItemSelectionController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly InventoryItemSelectionFactory _inventoryItemSelectionFactory;
        private readonly IUserInventoryService _userInventoryService;

        private List<InventoryItemSelectionView> _itemSelectionViews = new();
        private SelectedItemModel _selectedItemModel;

        private ShopItemSelectionWindow _window;
    
        public ItemSelectionController(IUiService uiService, InventoryItemSelectionFactory inventoryItemSelectionFactory, IUserInventoryService userInventoryService)
        {
            _uiService = uiService;
            _inventoryItemSelectionFactory = inventoryItemSelectionFactory;
            _userInventoryService = userInventoryService;
        }
    
        public override UniTask RunController(CancellationToken cancellationToken)
        {
            CreatePopup();
            return base.RunController(cancellationToken);
        }

        public override UniTask StopController()
        {
            _window?.DestroyWindow();
            return base.StopController();
        }

        public void SetItemModel(SelectedItemModel model) => _selectedItemModel = model;
    
        private async void CreatePopup()
        {
            _window = await _uiService.ShowWindow(ConstPopups.ShopItemSelectionPopup) as ShopItemSelectionWindow;

            _itemSelectionViews = _inventoryItemSelectionFactory.GetItemSelectionViews();
            _window.ParentChoices(_itemSelectionViews);
            _window.SetUsedSprite(_selectedItemModel.ItemSprite);

            foreach (var item in _itemSelectionViews)
                item.OnPressed += ProcessNewItemSelection;
        
            _window.OnRandomButtonPressed += SelectRandomNewItem;
        
            _window.OnClosePressed += () =>
            {
                StopController();
            };
        }

        private void SelectRandomNewItem()
        {
            var random = _itemSelectionViews[Random.Range(0, _itemSelectionViews.Count)];
            ProcessNewItemSelection(random.Id, random.Sprite);
        }

        private void ProcessNewItemSelection(int newItemID, Sprite sprite)
        {
            _selectedItemModel.UpdateShopItemID(newItemID, sprite);
            _userInventoryService.GetUsedGameItemIDs()[_selectedItemModel.ItemSlotID] = newItemID;
            StopController();
        }
    }
}

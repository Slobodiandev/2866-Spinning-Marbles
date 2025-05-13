using System.Collections.Generic;
using Core;
using Runtime.Gameplay.Services.SettingsProvider;
using Runtime.Gameplay.Services.UserData.Data;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Inventory
{
    public class SelectedItemsFactory : IInitializable
    {
        private readonly IAssetProvider _assetProvider;
        private readonly GameObjectFactory _factory;
        private readonly IUserInventoryService _userInventoryService;
    
        private GameObject _inventoryItemPrefab;
    
        public SelectedItemsFactory(IAssetProvider assetProvider, GameObjectFactory factory, 
            IUserInventoryService userInventoryService)
        {
            _assetProvider = assetProvider;
            _factory = factory;
            _userInventoryService = userInventoryService;
        }
    
        public async void Initialize()
        {
            _inventoryItemPrefab = await _assetProvider.LoadAsset<GameObject>(ConstPrefabs.InventoryItemDisplay);
        }

        public List<InventoryItemDisplay> CreateInventoryItemDisplayList()
        {
            List<InventoryItemDisplay> inventoryItemDisplayList = new ();

            var usedItemIds = _userInventoryService.GetUsedGameItemIDs();
            var usedItems = _userInventoryService.GetUsedGameItems();
        
            for (int i = 0; i < usedItemIds.Count; i++)
            {
                int itemID = usedItemIds[i];

                var model = new SelectedItemModel(i, itemID, usedItems[i].ItemSprite);
            
                var display = _factory.Create<InventoryItemDisplay>(_inventoryItemPrefab);
                display.Initialize(model);
            
                inventoryItemDisplayList.Add(display);
            }
        
            return inventoryItemDisplayList;
        }
    }
}

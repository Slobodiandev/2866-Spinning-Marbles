using System.Collections.Generic;
using System.Linq;
using Core;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.Services.SettingsProvider;
using Runtime.Gameplay.Services.UserData.Data;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Inventory
{
    public class InventoryItemSelectionFactory : IInitializable
    {
        private readonly GameObjectFactory _factory;
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigProvider _configProvider;
        private readonly IUserInventoryService _userInventoryService;

        private GameObject _prefab;
    
        public InventoryItemSelectionFactory(GameObjectFactory factory, IConfigProvider configProvider,
            IUserInventoryService userInventoryService, IAssetProvider assetProvider)
        {
            _factory = factory;
            _configProvider = configProvider;
            _userInventoryService = userInventoryService;
            _assetProvider = assetProvider;
        }

        public async void Initialize()
        {
            _prefab = await _assetProvider.LoadAsset<GameObject>(ConstPrefabs.InventoryItemSelectionView);
        }

        public List<InventoryItemSelectionView> GetItemSelectionViews()
        {
            List<InventoryItemSelectionView> itemSelectionViews = new List<InventoryItemSelectionView>();

            var shopSetup = _configProvider.Get<ShopSetup>();
            var itemIDs = GetUnusedItemIDs();

            for (int i = 0; i < itemIDs.Count; i++)
            {
                var display = _factory.Create<InventoryItemSelectionView>(_prefab);
                int id = itemIDs[i];
                display.Initialize(shopSetup.ShopItems[id].ItemSprite, id);
            
                itemSelectionViews.Add(display);
            }
        
            return itemSelectionViews;
        }

        private List<int> GetUnusedItemIDs()
        {
            var usedItemIDs = _userInventoryService.GetUsedGameItemIDs();
            var purchasedItemIDs = _userInventoryService.GetPurchasedGameItemsIDs();
        
            return purchasedItemIDs.Except(usedItemIDs).ToList();
        }
    }
}

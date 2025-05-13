using System;
using System.Collections.Generic;
using Core;
using Runtime.Gameplay.SeparateSystems.ShopSystem;

namespace Runtime.Gameplay.Services.UserData.Data
{
    public class UserInventoryService : IUserInventoryService
    {
        private readonly UserDataService _userDataService;
        private readonly IConfigProvider _configProvider;

        public event Action<int> BalanceChangedEvent;

        public UserInventoryService(UserDataService userDataService,
            IConfigProvider configProvider)
        {
            _userDataService = userDataService;
            _configProvider = configProvider;
        }

        public void SetBalance(int balance)
        {
            _userDataService.RetrieveUserData().UserInventory.Balance = balance;
            BalanceChangedEvent?.Invoke(balance);
        }

        public void AddBalance(int amount)
        { 
            int balance = _userDataService.RetrieveUserData().UserInventory.Balance + amount;
            SetBalance(balance);
        }

        public int GetBalance() => 
                _userDataService.RetrieveUserData().UserInventory.Balance;

        public List<ShopItem> GetUsedGameItems()
        {
            List<ShopItem> result = new();
            ShopSetup shopSetup = _configProvider.Get<ShopSetup>();
            var itemIds = GetUsedGameItemIDs();
            
            for (int i = 0; i < itemIds.Count; i++)
            {
                result.Add(shopSetup.ShopItems[itemIds[i]]);
            }
            
            return result;
        }

        public List<int> GetUsedGameItemIDs() => _userDataService.RetrieveUserData().UserInventory.UsedGameItemIDs;

        public void AddPurchasedGameItemID(int userGameItemID) =>
                _userDataService.RetrieveUserData().UserInventory.PurchasedGameItemsIDs.Add(userGameItemID);

        public List<int> GetPurchasedGameItemsIDs() =>
                _userDataService.RetrieveUserData().UserInventory.PurchasedGameItemsIDs;
    }
}
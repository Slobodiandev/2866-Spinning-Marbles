using System;
using System.Collections.Generic;
using Runtime.Gameplay.SeparateSystems.ShopSystem;

namespace Runtime.Gameplay.Services.UserData.Data
{
    public interface IUserInventoryService
    {
        event Action<int> BalanceChangedEvent;

        void SetBalance(int balance);
        
        void AddBalance(int amount);

        int GetBalance();
        
        List<ShopItem> GetUsedGameItems();

        List<int> GetUsedGameItemIDs();

        void AddPurchasedGameItemID(int userGameItemID);

        List<int> GetPurchasedGameItemsIDs();
    }
}
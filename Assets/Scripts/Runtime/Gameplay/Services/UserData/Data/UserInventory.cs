using System;
using System.Collections.Generic;

namespace Runtime.Gameplay.Services.UserData.Data
{
    [Serializable]
    public class UserInventory
    {
        public int Balance = 0;
        public List<int> UsedGameItemIDs = new () {0 ,1 , 2 , 3};
        public List<int> PurchasedGameItemsIDs = new() { 0, 1, 2, 3, 4, 5};
    }
}
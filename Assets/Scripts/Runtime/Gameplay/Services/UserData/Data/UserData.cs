using System;
using System.Collections.Generic;
using Runtime.Gameplay.SeparateSystems.UserAccount;

namespace Runtime.Gameplay.Services.UserData.Data
{
    [Serializable]
    public class UserData
    {
        public List<GameSessionData> GameSessionData = new List<GameSessionData>();
        public GameSettingsData _gameSettingsData = new GameSettingsData();
        public AppData AppData = new AppData();
        public UserProgressData UserProgressData = new UserProgressData();
        public UserInventory UserInventory = new UserInventory();
        public UserAccountData UserAccountData = new UserAccountData();
        public UserLoginData UserLoginData = new UserLoginData();
    }
}
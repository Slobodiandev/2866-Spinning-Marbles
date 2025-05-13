using System.Collections.Generic;
using System.Linq;
using Core;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.Services.UserData;

namespace Runtime.Gameplay.SeparateSystems.LevelSelection
{
    public class UserProgressService
    {
        private readonly UserDataService _userDataService;
        private readonly IConfigProvider _configProvider;
        private readonly GameData _gameData;
        private readonly LevelProgressTracker _levelProgressTracker;

        public UserProgressService(UserDataService userDataService, 
            IConfigProvider configProvider, GameData gameData,
            LevelProgressTracker levelProgressTracker)
        {
            _userDataService = userDataService;
            _configProvider = configProvider;
            _gameData = gameData;
            _levelProgressTracker = levelProgressTracker;
        }

        public bool NextLevelExists()
        {
            return _gameData.LevelID + 1 < GetLevelsAmountInGame();
        }

        public List<int> GetLevelsClearData() => _userDataService.RetrieveUserData().UserProgressData.LevelsClearList;

        public void SaveProgress()
        {
            int currentLevelID = _gameData.LevelID;
        
            RecordLevelClear(currentLevelID);
        
            var progressData = _userDataService.RetrieveUserData().UserProgressData;
        
            int lastUnlockedLevel = GetLastUnlockedLevelID();

            if (currentLevelID != lastUnlockedLevel)
                return;

            if (currentLevelID + 1 >= GetLevelsAmountInGame())
                return;

            progressData.LastUnlockedLevelID = lastUnlockedLevel + 1;
        }

        private void RecordLevelClear(int currentLevelID)
        {
            int currentStars = _levelProgressTracker.StarsEarned;
            var clearList = _userDataService.RetrieveUserData().UserProgressData.LevelsClearList;
        
            if(clearList.Count == currentLevelID)
                clearList.Add(currentStars);
            else
            {
                int lastStars = clearList[currentLevelID];
                if(currentStars > lastStars)
                    clearList[currentLevelID] = currentStars;
            }
        }

        public int GetTotalStars() => _userDataService.RetrieveUserData().UserProgressData.LevelsClearList.Sum();
    
        public int GetLastUnlockedLevelID() => 
            _userDataService.RetrieveUserData().UserProgressData.LastUnlockedLevelID;

        private int GetLevelsAmountInGame() => 
            _configProvider.Get<GameConfig>().LevelConfigs.Count;
    }
}

using System;
using System.Collections.Generic;

namespace Runtime.Gameplay.Services.UserData.Data
{
    [Serializable]
    public class UserProgressData
    {
        public int LastUnlockedLevelID = 0;
        public List<int> LevelsClearList = new();
    }
}
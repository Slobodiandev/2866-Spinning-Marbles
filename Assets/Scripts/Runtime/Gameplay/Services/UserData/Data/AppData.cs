using System;

namespace Runtime.Gameplay.Services.UserData.Data
{
    [Serializable]
    public class AppData
    {
        public int SessionNumber = 0;
        public bool IsAdb = false;
    }
}
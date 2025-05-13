using System;
using Runtime.Gameplay.Services.UserData;

namespace Runtime.Gameplay.DailyLogin
{
    public class DailyLoginService
    {
        private readonly UserDataService _userDataService;

        public DailyLoginService(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public bool SpinAvailable()
        {
            var lastLoginDateString  = GetSavedTime();
            if (lastLoginDateString == String.Empty)
                return true;
         
            var lastLoginDate = Convert.ToDateTime(lastLoginDateString);
            return DateTime.Now.Date > lastLoginDate.Date;
        }
    
        public void RecordSpinDate()
        {
            _userDataService.RetrieveUserData().UserLoginData.LastLoginDateString = DateTime.Now.ToString();
            _userDataService.SaveUserData();
        }
    
        private string GetSavedTime() => _userDataService.RetrieveUserData().UserLoginData.LastLoginDateString;
    }
}

using System.Text.RegularExpressions;
using Core;

namespace Runtime.Gameplay.SeparateSystems.UserAccount
{
    public class UserDataValidationService
    {
        private readonly IConfigProvider _configProvider;

        public UserDataValidationService(IConfigProvider configProvider)
        {
            _configProvider = configProvider;
        }

        public bool IsNameValid(string name)
        {
            UserDataValidationConfig userDataValidationConfig = _configProvider.Get<UserDataValidationConfig>();
            return !string.IsNullOrWhiteSpace(name) && 
                   Regex.IsMatch(name, userDataValidationConfig.UsernameRegex);
        }
        
        public bool IsAgeValid(string ageText)
        {
            if (!int.TryParse(ageText, out int age))
                return false;
            
            UserDataValidationConfig userDataValidationConfig = _configProvider.Get<UserDataValidationConfig>();
            return age >= userDataValidationConfig.MinAge && 
                   age <= userDataValidationConfig.MaxAge;
        }
        
        public bool IsGenderValid(string gender)
        {
            UserDataValidationConfig userDataValidationConfig = _configProvider.Get<UserDataValidationConfig>();
            return !string.IsNullOrWhiteSpace(gender) &&  
                   Regex.IsMatch(gender, userDataValidationConfig.GenderRegex);
        }
    }   
}

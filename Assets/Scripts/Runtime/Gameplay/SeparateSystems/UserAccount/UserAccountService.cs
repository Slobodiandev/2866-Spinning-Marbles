using System;
using Core;
using Runtime.Gameplay.Services.UserData;
using UnityEngine;

namespace Runtime.Gameplay.SeparateSystems.UserAccount
{
    public class UserAccountService
    {
        private readonly UserDataService _userDataService;
        private readonly IConfigProvider _configProvider;
        private readonly ImageProcessingService _imageProcessingService;
    
        public UserAccountService(UserDataService userDataService, 
            IConfigProvider configProvider, 
            ImageProcessingService imageProcessingService)
        {
            _userDataService = userDataService;
            _configProvider = configProvider;
            _imageProcessingService = imageProcessingService;
        }
    
        public UserAccountData GetAccountDataCopy()
        {
            return _userDataService.RetrieveUserData().UserAccountData.Copy();
        }

        public void SaveAccountData(UserAccountData modifiedData)
        {
            var origData = _userDataService.RetrieveUserData().UserAccountData;

            foreach (var field in typeof(UserAccountData).GetFields())
                field.SetValue(origData, field.GetValue(modifiedData));

            _userDataService.SaveUserData();
        }

        public Sprite GetUsedAvatarSprite(bool trySetDefaultIfNull = false)
        {
            if (!AvatarExists())
            {
                if (trySetDefaultIfNull)
                    TrySetDefaultSprite();
                else
                    return null;
            }
            
            return _imageProcessingService.CreateAvatarSprite(GetAvatarBase64());
        }

        [Tooltip("Pass in the selected avatar and assign the returned string to the account data")]
        public string ConvertToBase64(Sprite sprite, int maxSize = 512) =>
            _imageProcessingService.ConvertToBase64(sprite, maxSize);

        private void TrySetDefaultSprite()
        {
            var avatarsConfig = _configProvider.Get<DefaultAvatarsConfig>();
            
            if (avatarsConfig == null)
                return;
        
            if(avatarsConfig.Avatars.Count == 0)
                return;
        
            _userDataService.RetrieveUserData().UserAccountData.AvatarBase64 = ConvertToBase64(avatarsConfig.Avatars[0]);
            _userDataService.SaveUserData();
        }


        private bool AvatarExists() => _userDataService.RetrieveUserData().UserAccountData.AvatarBase64 != String.Empty;
        
        private string GetAvatarBase64() => _userDataService.RetrieveUserData().UserAccountData.AvatarBase64;
    }
}

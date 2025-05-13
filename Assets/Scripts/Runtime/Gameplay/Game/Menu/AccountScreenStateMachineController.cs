using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.SeparateSystems.UserAccount;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;
using UnityEngine;

namespace Runtime.Gameplay.Game.Menu
{
    public class AccountScreenStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly UserAccountService _userAccountService;
        private readonly AvatarSelectionService _avatarSelectionService;
        private readonly UserDataValidationService _userDataValidationService;

        private AccountScreen _screen;

        private UserAccountData _modifiedData;
        
        private LastScreen _lastScreen;

        public AccountScreenStateMachineController(ICustomLogger customLogger, IUiService uiService,
            UserAccountService userAccountService,
            AvatarSelectionService avatarSelectionService,
            UserDataValidationService userDataValidationService) : base(customLogger)
        {
            _uiService = uiService;
            _userAccountService = userAccountService;
            _avatarSelectionService = avatarSelectionService;
            _userDataValidationService = userDataValidationService;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CopyData();
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.AccountScreen);
        }

        public void SetLastScreen(LastScreen lastScreen) => _lastScreen = lastScreen;
        
        private void CopyData() => _modifiedData = _userAccountService.GetAccountDataCopy();
        
        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<AccountScreen>(ConstScreens.AccountScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
            _screen.SetData(_modifiedData);
            _screen.SetAvatar(_userAccountService.GetUsedAvatarSprite(false));
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += GoBack;
            _screen.OnSavePressed += SaveAndLeave;
            _screen.OnChangeAvatarPressed += SelectNewAvatar;
            _screen.OnNameChanged += ValidateName;
        }

        private async void GoBack()
        {
            if (_lastScreen == LastScreen.Menu)
                await GoToState<MenuStateMachineController>();
            else
                await GoToState<SettingsScreenMachineController>();
        }

        private void SaveAndLeave()
        {
            _userAccountService.SaveAccountData(_modifiedData);
            GoBack();
        }

        private async void SelectNewAvatar()
        {
            Sprite newAvatar = await _avatarSelectionService.PickImage(512, CancellationToken.None);

            if (newAvatar != null)
            {
                _modifiedData.AvatarBase64 = _userAccountService.ConvertToBase64(newAvatar);
                _screen.SetAvatar(newAvatar);
            }
        }

        private void ValidateName(string value)
        {
            if (!_userDataValidationService.IsNameValid(value))
                _screen.SetData(_modifiedData);
            else
                _modifiedData.Username = value;
        }
        
        public enum LastScreen
        {
            Settings,
            Menu
        }
    }
}
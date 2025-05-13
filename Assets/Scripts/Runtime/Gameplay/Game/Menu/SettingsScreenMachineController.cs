using System.Threading;
using Core;
using Core.Services.Audio;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class SettingsScreenMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;
        private readonly AccountScreenStateMachineController _accountScreenStateMachineController;

        private SettingsScreen _screen;
        
        private float _originalSoundVolume;
        private float _originalMusicVolume;

        public SettingsScreenMachineController(ICustomLogger customLogger, IUiService uiService, UserDataService userDataService, 
            IAudioService audioService, AccountScreenStateMachineController accountScreenStateMachineController) : base(customLogger)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
            _accountScreenStateMachineController = accountScreenStateMachineController;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CopyOriginalData();
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.SettingsScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<SettingsScreen>(ConstScreens.SettingsScreen);
            _screen.Initialize(_userDataService.RetrieveUserData()._gameSettingsData);
            _screen.ShowAsync().Forget();
        }

        private void CopyOriginalData()
        {
            var settingsData = _userDataService.RetrieveUserData()._gameSettingsData;
            _originalSoundVolume = settingsData.SoundVolume;
            _originalMusicVolume = settingsData.MusicVolume;
        }
        
        private void SubscribeToEvents()
        {   
            _screen.OnBackPressed += async () =>
            {
                ResetData();
                await GoToState<MenuStateMachineController>();
            };
            _screen.OnProfilePressed += async () =>
            {
                _accountScreenStateMachineController.SetLastScreen(AccountScreenStateMachineController.LastScreen.Settings);
                ResetData();
                await GoToState<AccountScreenStateMachineController>();
            };
            _screen.OnSavePressed += async () =>
            {
                await GoToState<MenuStateMachineController>();
            };

            _screen.OnSoundVolumeChanged += OnChangeSoundVolume;
            _screen.OnMusicVolumeChanged += OnChangeMusicVolume;
        }

        private void ResetData()
        {
            OnChangeSoundVolume(_originalSoundVolume);
            OnChangeMusicVolume(_originalMusicVolume);
        }
        
        private void OnChangeSoundVolume(float value)
        {
            _audioService.SetVolume(AudioType.Sound, value);
            var userData = _userDataService.RetrieveUserData();
            userData._gameSettingsData.SoundVolume = value;
        }

        private void OnChangeMusicVolume(float value)
        {
            _audioService.SetVolume(AudioType.Music, value);
            var userData = _userDataService.RetrieveUserData();
            userData._gameSettingsData.MusicVolume = value;
        }
    }
}
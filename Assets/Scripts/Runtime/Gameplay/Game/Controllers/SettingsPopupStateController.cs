using System.Threading;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData;
using Runtime.Gameplay.UI.Popup;
using Runtime.Gameplay.UI.Popup.Data;
using UnityEngine;
using AudioType = Core.Services.Audio.AudioType;

namespace Runtime.Gameplay.Game.Controllers
{
    public sealed class SettingsPopupStateController : BaseController
    {
        private readonly IUiService _uiService;
        private readonly UserDataService _userDataService;
        private readonly IAudioService _audioService;

        public SettingsPopupStateController(IUiService uiService, UserDataService userDataService, IAudioService audioService)
        {
            _uiService = uiService;
            _userDataService = userDataService;
            _audioService = audioService;
        }

        public override UniTask RunController(CancellationToken cancellationToken)
        {
            base.RunController(cancellationToken);

            Time.timeScale = 0;
            SettingsWindow settingsWindow = _uiService.GetWindow<SettingsWindow>(ConstPopups.SettingsPopup);

            settingsWindow.SoundVolumeChangeEvent += UpdateSoundVolume;
            settingsWindow.MusicVolumeChangeEvent += UpdateMusicVolume;
            settingsWindow.OnClosePressed += () =>
            {
                Time.timeScale = 1;
                settingsWindow.DestroyWindow();
            };

            var userData = _userDataService.RetrieveUserData();

            var soundVolume = userData._gameSettingsData.SoundVolume;
            var musicVolume = userData._gameSettingsData.MusicVolume;

            settingsWindow.ShowWindow(new SettingsWindowData(soundVolume, musicVolume), cancellationToken).Forget();
            CurrentStates = ControllerStates.Complete;
            return UniTask.CompletedTask;
        }
        
        
        private void UpdateSoundVolume(float value)
        {
            _audioService.SetVolume(AudioType.Sound, value);
            var userData = _userDataService.RetrieveUserData();
            userData._gameSettingsData.SoundVolume = value;
        }

        private void UpdateMusicVolume(float value)
        {
            _audioService.SetVolume(AudioType.Music, value);
            var userData = _userDataService.RetrieveUserData();
            userData._gameSettingsData.MusicVolume = value;
        }
    }
}
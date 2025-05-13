using System;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.Audio;
using Runtime.Gameplay.UI.Popup.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _soundVolumeToggle;
        [SerializeField] private Slider _musicVolumeToggle;

        public event Action<float> SoundVolumeChangeEvent;
        public event Action<float> MusicVolumeChangeEvent;

        public event Action OnClosePressed;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            SettingsWindowData settingsWindowData = data as SettingsWindowData;

            var soundVolume = settingsWindowData.SoundVolume;
            _soundVolumeToggle.onValueChanged.Invoke(soundVolume);
            _soundVolumeToggle.value = soundVolume;
            
            var musicVolume = settingsWindowData.MusicVolume;
            _musicVolumeToggle.onValueChanged.Invoke(musicVolume);
            _musicVolumeToggle.value = musicVolume;

            _closeButton.onClick.AddListener(() =>
            {
                AudioService.PlaySound(ConstAudio.PressButtonSound);
                OnClosePressed?.Invoke();
            });

            _soundVolumeToggle.onValueChanged.AddListener(OnSoundVolumeToggleValueChanged);
            _musicVolumeToggle.onValueChanged.AddListener(OnMusicVolumeToggleValueChanged);

            AudioService.PlaySound(ConstAudio.OpenPopupSound);

            return base.ShowWindow(data, cancellationToken);
        }
        
        private void OnSoundVolumeToggleValueChanged(float value)
        {
            SoundVolumeChangeEvent?.Invoke(value);
        }

        private void OnMusicVolumeToggleValueChanged(float value)
        {
            MusicVolumeChangeEvent?.Invoke(value);
        }
    }
}
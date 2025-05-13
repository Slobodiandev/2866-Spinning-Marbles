using System;
using Runtime.Gameplay.Services.UserData.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Screen
{
    public class SettingsScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _profileButton;
        [SerializeField] private Button _saveButton;

        [SerializeField] private Slider _soundVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        
        public event Action OnBackPressed;
        public event Action OnProfilePressed;
        public event Action OnSavePressed;

        public event Action<float> OnSoundVolumeChanged;
        public event Action<float> OnMusicVolumeChanged;
        
        public void Initialize(GameSettingsData data)
        {
            SubscribeToEvents();

            SetSliderData(data);
        }

        private void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _profileButton.onClick.AddListener(() => OnProfilePressed?.Invoke());
            _saveButton.onClick.AddListener(() => OnSavePressed?.Invoke());
            _soundVolumeSlider.onValueChanged.AddListener((value) => OnSoundVolumeChanged?.Invoke(value));
            _musicVolumeSlider.onValueChanged.AddListener((value) => OnMusicVolumeChanged?.Invoke(value));
        }

        private void SetSliderData(GameSettingsData data)
        {
            _soundVolumeSlider.value = data.SoundVolume;
            _musicVolumeSlider.value = data.MusicVolume;
        }
    }
}
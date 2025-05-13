using Core.UI;

namespace Runtime.Gameplay.UI.Popup.Data
{
    public class SettingsWindowData : BaseWindowData
    {
        private float _soundVolume;
        private float _musicVolume;

        public float SoundVolume => _soundVolume;
        public float MusicVolume => _musicVolume;

        public SettingsWindowData(float soundVolume, float musicVolume)
        {
            _soundVolume = soundVolume;
            _musicVolume = musicVolume;
        }
    }
}
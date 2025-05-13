using System.Linq;
using System.Threading;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UserData;
using AudioType = Core.Services.Audio.AudioType;

namespace Runtime.Gameplay.Initialization.Controllers
{
    public class AudioSettingsBootstrapController : BaseController
    {
        private readonly IAudioService _audioService;
        private readonly IConfigProvider _staticConfigsService;
        private readonly UserDataService _userDataService;

        private CancellationTokenSource _cancellationTokenSource;

        public AudioSettingsBootstrapController(IAudioService audioService, IConfigProvider staticConfigsService, UserDataService userDataService)
        {
            _audioService = audioService;
            _staticConfigsService = staticConfigsService;
            _userDataService = userDataService;
        }

        public override UniTask RunController(CancellationToken cancellationToken)
        {
            base.RunController(cancellationToken);

            _cancellationTokenSource = new CancellationTokenSource();
            var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);

            LoadVolumeSettings();
            LoopMusicAsync(linkedTokenSource.Token).Forget();
            return UniTask.CompletedTask;
        }

        public override UniTask StopController()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            return base.StopController();
        }

        private async UniTask LoopMusicAsync(CancellationToken token)
        {
            var config = _staticConfigsService.Get<AudioConfig>();
            var musicEntries = config.Audio.Where(entry => entry.AudioType == AudioType.Music).ToList();
            var musicClips = musicEntries.Select(entry => entry.Clip).ToList();

            int totalClips = musicClips.Count;
            int currentIndex = 0;

            while (!token.IsCancellationRequested)
            {
                var currentClip = musicClips[currentIndex];
                _audioService.PlayMusic(currentClip);

                int waitTimeMs = ((int)currentClip.length * 1000) + 1000;
                await UniTask.Delay(waitTimeMs, cancellationToken: token);

                currentIndex = (currentIndex + 1) % totalClips;
            }
        }

        private void LoadVolumeSettings()
        {
            _audioService.SetVolume(AudioType.Sound, _userDataService.RetrieveUserData()._gameSettingsData.SoundVolume);
            _audioService.SetVolume(AudioType.Music, _userDataService.RetrieveUserData()._gameSettingsData.MusicVolume);
        }
    }
}
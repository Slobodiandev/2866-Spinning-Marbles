using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Screen
{
    public class SplashScreen : UiScreen
    {
        [SerializeField] private float _loadingDurationMin;
        [SerializeField] private float _loadingDurationMax;
        [SerializeField] private Slider _loadingBar;
        [SerializeField] private TextMeshProUGUI _loadingText;
        
        public override async UniTask HideAsync(CancellationToken cancellationToken = default)
        {
            await WaitSplashScreenAnimationFinish(cancellationToken);
            await base.HideAsync(cancellationToken);
        }

        private async UniTask WaitSplashScreenAnimationFinish(CancellationToken cancellationToken)
        {
            float duration = Random.Range(_loadingDurationMin, _loadingDurationMax);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float progress = Mathf.SmoothStep(0f, 1f, elapsed / duration);

                _loadingBar.value = progress;
                _loadingText.text = $"{Mathf.RoundToInt(progress * 100)}%";

                elapsed += Time.deltaTime;
                await UniTask.Yield(cancellationToken);
            }

            _loadingBar.value = 1f;
            _loadingText.text = "100%";
        }
    }
}
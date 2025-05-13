using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class GameResultWindow : BaseWindow
    {
        private const float TextAnimationTime = 1f;
        private const float StarsAnimationTime = 1f;
        private readonly Vector3 TargetScale = Vector3.one * 1.003f;
        
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private ClearData[] _clearDataArray;
        [SerializeField] private Sprite _checkSprite;
        [SerializeField] private Sprite _crossSprite;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _activeStar;
        
        public event Action OnNextLevelButtonPressed;
        public event Action OnRetryButtonPressed;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _nextLevelButton.onClick.AddListener(() => OnNextLevelButtonPressed?.Invoke());
            _retryButton.onClick.AddListener(() => OnRetryButtonPressed?.Invoke());
            
            return base.ShowWindow(data, cancellationToken);
        }

        public void SetClearData(List<bool> clearData, int stars, int coinsEarned)
        {
            for (int i = 0; i < _clearDataArray.Length; i++)
                _clearDataArray[i].ClearDataImage.sprite = clearData[i] ? _checkSprite : _crossSprite;
            
            for (int i = 0; i < stars; i++)
            {
                if (stars > i)
                    _stars[i].sprite = _activeStar;
            }

            _stars[1].transform.DOPunchScale(TargetScale, StarsAnimationTime).SetUpdate(true).SetLink(gameObject);
            
            _nextLevelButton.gameObject.SetActive(stars >= 1);
            _retryButton.gameObject.SetActive(stars == 0);
            _resultText.text = stars >= 1 ? "YOU WIN!" : "No stars collected...";

            StartCoroutine(CountUpRoutine(coinsEarned));
        }

        private IEnumerator CountUpRoutine(int targetValue)
        {
            float elapsed = 0f;
            string prefix = "Coins: +";
            while (elapsed < TextAnimationTime)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / TextAnimationTime);

                t = Mathf.Sin(t * Mathf.PI * 0.5f);

                int currentValue = Mathf.RoundToInt(Mathf.Lerp(0, targetValue, t));
                _rewardText.text = prefix + currentValue;
                
                yield return null;
            }

            _rewardText.text = prefix + targetValue;
        }

        public void SetSkins(List<Sprite> skins)
        {
            for(int i = 0; i < _clearDataArray.Length; i++)
                _clearDataArray[i].BallTypeImage.sprite = skins[i];
        }

        [Serializable]
        private class ClearData
        {
            public Image BallTypeImage;
            public Image ClearDataImage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class LoseWindow : BaseWindow
    {
        private const float StarsAnimationTime = 1f;
        private readonly Vector3 TargetScale = Vector3.one * 1.003f;
        
        [SerializeField] private Button _retryButton;
        [SerializeField] private ClearData[] _clearDataArray;
        [SerializeField] private Sprite _checkSprite;
        [SerializeField] private Sprite _crossSprite;
        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _activeStar;
        
        public event Action OnRetryButtonPressed;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _retryButton.onClick.AddListener(() => OnRetryButtonPressed?.Invoke());
            
            return base.ShowWindow(data, cancellationToken);
        }

        public void SetClearData(List<bool> clearData, int stars)
        {
            for (int i = 0; i < _clearDataArray.Length; i++)
                _clearDataArray[i].ClearDataImage.sprite = clearData[i] ? _checkSprite : _crossSprite;
            
            for (int i = 0; i < stars; i++)
            {
                if (stars > i)
                    _stars[i].sprite = _activeStar;
            }

            _stars[1].transform.DOPunchScale(TargetScale, StarsAnimationTime).SetUpdate(true).SetLink(gameObject);
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
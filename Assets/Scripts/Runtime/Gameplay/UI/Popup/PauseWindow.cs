using System;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class PauseWindow : BaseWindow
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _homeButton;
        [SerializeField] private TextMeshProUGUI _starsText;
        [SerializeField] private TextMeshProUGUI _ballsText;
        
        public event Action OnResumeButtonPressed;
        public event Action OnHomeButtonPressed;
        
        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _resumeButton.onClick.AddListener(() => OnResumeButtonPressed?.Invoke());
            _homeButton.onClick.AddListener(() => OnHomeButtonPressed?.Invoke());
            return base.ShowWindow(data, cancellationToken);
        }

        public void SetData(int stars, int balls)
        {
            _starsText.text = stars.ToString();
            _ballsText.text = balls.ToString();
        }
    }
}
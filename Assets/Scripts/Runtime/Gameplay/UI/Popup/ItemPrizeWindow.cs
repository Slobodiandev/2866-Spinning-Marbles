using System;
using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class ItemPrizeWindow : BaseWindow
    {
        [SerializeField] private Button _claimButton;
        [SerializeField] private TextMeshProUGUI _rewardText;
        
        public event Action OnClaimButtonPressed;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _claimButton.onClick.AddListener(() => OnClaimButtonPressed?.Invoke());
            return base.ShowWindow(data, cancellationToken);
        }
        
        public void SetReward(int reward) => _rewardText.text = "x" + reward.ToString();
    }
}
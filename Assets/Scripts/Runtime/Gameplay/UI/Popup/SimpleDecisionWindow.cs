using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.UI.Popup.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class SimpleDecisionWindow : BaseWindow
    {
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TextMeshProUGUI _message;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            SimpleDecisionWindowData simpleDecisionWindowData = data as SimpleDecisionWindowData;

            _message.text = simpleDecisionWindowData.Message;
            _confirmButton.onClick.AddListener(() =>
            {
                simpleDecisionWindowData?.PressOkEvent?.Invoke();
                HideWindow();
            });

            _cancelButton.onClick.AddListener(HideWindow);

            return base.ShowWindow(data, cancellationToken);
        }
    }
}
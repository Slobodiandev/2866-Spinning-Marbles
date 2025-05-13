using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.UI.Popup.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class MessageWindow : BaseWindow
    {
        [SerializeField] private Button _okButton;
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private TextMeshProUGUI _message;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            MessageWindowData messageWindowData = data as MessageWindowData;

            _message.text = messageWindowData.Message;

            if (messageWindowData.IsShowButton)
            {
                _okButton.gameObject.SetActive(true);
                _okButton.onClick.AddListener(HideWindow);
                _backgroundButton.gameObject.SetActive(false);
            }
            else
            {
                _okButton.gameObject.SetActive(false);
                _backgroundButton.onClick.AddListener(HideWindow);
            }

            return base.ShowWindow(data, cancellationToken);
        }
    }
}
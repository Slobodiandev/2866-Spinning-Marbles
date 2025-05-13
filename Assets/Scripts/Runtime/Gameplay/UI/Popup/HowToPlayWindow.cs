using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class HowToPlayWindow : BaseWindow
    {
        [SerializeField] private Button _closeButton;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            _closeButton.onClick.AddListener(DestroyWindow);
            return base.ShowWindow(data, cancellationToken);
        }

        public override void DestroyWindow()
        {
            Time.timeScale = 1;
            base.DestroyWindow();
        }
    }
}
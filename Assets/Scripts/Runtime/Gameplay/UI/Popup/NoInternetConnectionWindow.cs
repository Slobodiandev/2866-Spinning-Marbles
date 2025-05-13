using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class NoInternetConnectionWindow : BaseWindow
    {
        [SerializeField] private Button _okButton;

        private UniTaskCompletionSource _completionSource;

        public override async UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _completionSource = new UniTaskCompletionSource();
            _okButton.onClick.AddListener(HideWindow);

            await _completionSource.Task;

            DestroyWindow();
        }

        public override void HideWindow()
        {
            _completionSource?.TrySetResult();
        }
    }
}
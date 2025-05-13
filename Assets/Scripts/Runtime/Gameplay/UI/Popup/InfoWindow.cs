using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Popup
{
    public class InfoWindow : BaseWindow
    {
        [SerializeField] private Button _okButton;

        public override UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            _okButton.onClick.AddListener(DestroyWindow);

            return base.ShowWindow(data, cancellationToken);
        }
    }
}
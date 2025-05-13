using System.Threading;
using Core.UI;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.UI.Screen;
using UnityEngine;

namespace Runtime.Gameplay.Services.UI
{
    public interface IUiService
    {
        UniTask Initialize();
        bool IsScreenActive(string id);
        T GetScreen<T>(string id) where T : UiScreen;
        UniTask ShowScreen(string id, CancellationToken cancellationToken = default);
        UniTask HideScreen(string id, CancellationToken cancellationToken = default);
        void HideScreenImmediately(string id);
        UniTask<BaseWindow> ShowWindow(string id, BaseWindowData data = null, CancellationToken cancellationToken = default);
        T GetWindow<T>(string id) where T : BaseWindow;
        void HideAllScreensImmediately();
        UniTask HideAllAsyncScreens(CancellationToken cancellationToken = default);
        UniTask FadeInAsync(
            Color? color = null,
            float? duration = null,
            CancellationToken cancellationToken = default
        );

        UniTask FadeOutAsync(
            Color? color = null,
            float? duration = null,
            CancellationToken cancellationToken = default
        );
    }
}
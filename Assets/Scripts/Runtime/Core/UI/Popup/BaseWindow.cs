using System;
using System.Threading;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Core.UI
{
    public class BaseWindow : MonoBehaviour
    {
        private const string OpenPopupAudio = "OpenPopup";
        private const string ClosePopupAudio = "ClosePopupSound";
        
        [SerializeField] protected string _id;

        protected IAudioService AudioService;

        public UnityEvent ShowEvent;
        public UnityEvent HideEvent;
        public UnityEvent HideImmediatelyEvent;
        
        public string Id => _id;

        [Inject]
        public void Construct(IAudioService audioService)
        {
            AudioService = audioService;
        }

        public virtual UniTask ShowWindow(BaseWindowData data, CancellationToken cancellationToken = default)
        {
            PlayAudio(OpenPopupAudio);
            ShowEvent?.Invoke();
            return UniTask.CompletedTask;
        }

        public virtual void HideWindow()
        {
            HideEvent?.Invoke();
        }

        public virtual void HideWindowImmediately()
        {
            HideImmediatelyEvent?.Invoke();
        }

        public virtual void DestroyWindow()
        {
            PlayAudio(ClosePopupAudio);
            Destroy(gameObject);
        }

        protected void PlayAudio(string soundName)
        {
            AudioService.PlaySound(soundName);
        }
    }
}
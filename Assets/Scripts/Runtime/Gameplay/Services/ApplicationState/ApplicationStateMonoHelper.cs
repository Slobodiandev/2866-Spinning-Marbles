using System;
using UnityEngine;

namespace Runtime.Gameplay.Services.ApplicationState
{
    public class ApplicationStateMonoHelper : MonoBehaviour
    {
        public event Action ApplicationQuitEvent;
        public event Action<bool> ApplicationPauseEvent;

        private void OnApplicationQuit()
        {
            ApplicationQuitEvent?.Invoke();
        }
        
        private void OnApplicationPause(bool isPause)
        {
            ApplicationPauseEvent?.Invoke(isPause);
        }
    }
}
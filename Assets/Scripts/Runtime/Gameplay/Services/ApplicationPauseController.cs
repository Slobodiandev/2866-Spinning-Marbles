using Runtime.Gameplay.Services.UI;
using UnityEngine;

namespace Runtime.Gameplay.Services
{
    public static class ApplicationPauseController
    {
        private static bool _pausedFromPauseTaker = false;
        
        public static void TakePause(GameStateTypeId gameState)
        {
            if (Time.timeScale == 1 && gameState == GameStateTypeId.PausedState)
                _pausedFromPauseTaker = true;

            if (_pausedFromPauseTaker)
                Time.timeScale = gameState == GameStateTypeId.RunningState ? 1 : 0;
            
            if(gameState == GameStateTypeId.RunningState)
                _pausedFromPauseTaker = false;
        }
    }
}
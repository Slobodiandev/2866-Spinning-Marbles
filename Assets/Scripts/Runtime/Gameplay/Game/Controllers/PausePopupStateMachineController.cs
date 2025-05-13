using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Popup;
using UnityEngine;

namespace Runtime.Gameplay.Game.Controllers
{
    public class PausePopupStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly GameData _gameData;
        private readonly LevelProgressTracker _levelProgressTracker;
    
        public PausePopupStateMachineController(ICustomLogger customLogger, IUiService uiService, GameData gameData, LevelProgressTracker levelProgressTracker) : base(customLogger)
        {
            _uiService = uiService;
            _gameData = gameData;
            _levelProgressTracker = levelProgressTracker;
        }

        public override async UniTask EnterState(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
        
            PauseWindow window = await _uiService.ShowWindow(ConstPopups.PausePopup) as PauseWindow;

            int ballsLeft = _gameData.MaxLaunchedBalls - _gameData.LaunchedBalls;

            window.SetData(_levelProgressTracker.StarsEarned, ballsLeft);
        
            window.OnResumeButtonPressed += () =>
            {
                Time.timeScale = 1;
                window.DestroyWindow();
            };
        
            window.OnHomeButtonPressed += async () =>
            {
                Time.timeScale = 1;
                window.DestroyWindow();
                await GoToState<MainScreenMachineController>();
            };
        }
    }
}

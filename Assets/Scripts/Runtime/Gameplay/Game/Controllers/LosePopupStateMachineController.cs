using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData.Data;
using Runtime.Gameplay.UI.Popup;
using UnityEngine;

namespace Runtime.Gameplay.Game.Controllers
{
    public class LosePopupStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly GameData _gameData;
        private readonly LevelProgressTracker _levelProgressTracker;
        private readonly IUserInventoryService _userInventoryService;

        public LosePopupStateMachineController(ICustomLogger customLogger, LevelProgressTracker levelProgressTracker, GameData gameData, IUiService uiService, 
            IUserInventoryService userInventoryService) : base(customLogger)
        {
            _levelProgressTracker = levelProgressTracker;
            _uiService = uiService;
            _gameData = gameData;
            _userInventoryService = userInventoryService;
        }

        public override async UniTask EnterState(CancellationToken cancellationToken = default)
        {
            Time.timeScale = 0;
            LoseWindow window = await _uiService.ShowWindow(ConstPopups.LosePopup) as LoseWindow;
        
            window.SetSkins(GetBallSkins());
            window.SetClearData(GetClearData(), _levelProgressTracker.StarsEarned);
        
            window.OnRetryButtonPressed += async () =>
            {
                Time.timeScale = 1;
                window.DestroyWindow();
                await GoToState<GameplayScreenMachineController>();
            };
        }

        private List<bool> GetClearData()
        {
            List<bool> clearData = new (4);
            var ballsLeft = _gameData.ActiveBalls;

            for (int i = 0; i < 4; i++)
                clearData.Add(!ballsLeft.Any(x => x.GetBallTypeID() == i));

            return clearData;
        }

        private List<Sprite> GetBallSkins()
        {
            List<Sprite> result = new (4);
            var usedSkins = _userInventoryService.GetUsedGameItems();

            for (int i = 0; i < 4; i++)
                result.Add(usedSkins[i].ItemSprite);
        
            return result;
        }
    }
}

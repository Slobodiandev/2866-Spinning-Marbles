using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.Gameplay.Visuals;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData.Data;
using Runtime.Gameplay.UI.Popup;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay
{
    public class GameResultMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly GameData _gameData;
        private readonly LevelProgressTracker _levelProgressTracker;
        private readonly GameEndAnimationController _animationController;
        private readonly IUserInventoryService _userInventoryService;
        private readonly UserProgressService _userProgressService;
        private readonly CoinsRewardCalculator _coinsRewardCalculator;

        public GameResultMachineController(ICustomLogger customLogger, LevelProgressTracker levelProgressTracker, GameData gameData, IUiService uiService, 
            GameEndAnimationController gameEndAnimationController, IUserInventoryService userInventoryService, 
            UserProgressService userProgressService, CoinsRewardCalculator coinsRewardCalculator) : base(customLogger)
        {
            _levelProgressTracker = levelProgressTracker;
            _uiService = uiService;
            _gameData = gameData;
            _animationController = gameEndAnimationController;
            _userInventoryService = userInventoryService;
            _userProgressService = userProgressService;
            _coinsRewardCalculator = coinsRewardCalculator;
        }

        public override async UniTask EnterState(CancellationToken cancellationToken = default)
        {
            await _animationController.PlayAnimation();

            Time.timeScale = 0;
            GameResultWindow window = await _uiService.ShowWindow(ConstPopups.GameResultPopup) as GameResultWindow;

            int reward = _coinsRewardCalculator.CalculateReward();
        
            window.SetSkins(GetBallSkins());
            window.SetClearData(GetClearData(), _levelProgressTracker.StarsEarned, reward);
        
            _userInventoryService.AddBalance(reward);

            if(_levelProgressTracker.StarsEarned > 0)
                _userProgressService.SaveProgress();
        
            window.OnNextLevelButtonPressed += async () =>
            {
                Time.timeScale = 1;
                window.DestroyWindow();

                if (_userProgressService.NextLevelExists())
                    _gameData.LevelID++;
            
                await GoToState<GameplayScreenMachineController>();
            };
        
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

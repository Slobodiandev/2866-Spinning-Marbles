using System.Collections.Generic;
using System.Linq;
using Core;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay
{
    public class GameSetupController
    {
        private readonly GameplaySystemsManager _gameplaySystemsManager;
        private readonly GameData _gameData;
        private readonly GameplayObjectsActivator _gameplayObjectsActivator;
        private readonly LevelIntitializer _levelIntitializer;
        private readonly IConfigProvider _configProvider;

        public GameSetupController(GameplaySystemsManager gameplaySystemsManager, GameData gameData, 
            GameplayObjectsActivator gameplayObjectsActivator, LevelIntitializer levelIntitializer, IConfigProvider configProvider)
        {
            _gameplaySystemsManager = gameplaySystemsManager;
            _gameData = gameData;
            _gameplayObjectsActivator = gameplayObjectsActivator;
            _levelIntitializer = levelIntitializer;
            _configProvider = configProvider;
        }

        public void SetupGame()
        {
            _levelIntitializer.SpawnLevel();
            SetupGameData();
            _gameplaySystemsManager.ResetAllSystems();
            _gameplaySystemsManager.EnableAllSystems(true);
            _gameplayObjectsActivator.Enable(true);
        }

        public void DisableGameSystems()
        {
            _gameplaySystemsManager.EnableAllSystems(false);
        }

        public void DisableGameplayObjects()
        {
            _gameplayObjectsActivator.Enable(false);
            _levelIntitializer.DestroyLevel();
        }

        private void SetupGameData()
        {
            var ballsFound = Object.FindObjectsOfType<BallTarget>(true).ToList();

            for (int i = 0; i < ballsFound.Count; i++)
            {
                if (ballsFound[i] is LaunchedBall ball)
                {
                    ballsFound.Remove(ball);
                    break;
                }
            }
        
            var ballsInLevel = new HashSet<BallTarget>(ballsFound);
            _gameData.ActiveBalls = ballsInLevel;
            _gameData.OriginalLevelBalls = new(ballsInLevel);
            _gameData.LevelBallsCount = ballsInLevel.Count;
            _gameData.LevelProgress = 0;
            _gameData.LaunchedBalls = 0;
            _gameData.MaxLaunchedBalls = _configProvider.Get<GameConfig>().LevelConfigs[_gameData.LevelID].BallsCount;
        }
    }
}

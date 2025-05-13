using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Configs;
using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Gameplay.Systems;
using Runtime.Gameplay.SeparateSystems.LevelSelection;

namespace Runtime.Gameplay.Gameplay.Misc
{
    public class LevelProgressTracker : IResettable
    {
        private readonly GameData _gameData;
        private readonly IConfigProvider _configProvider;
    
        public event Action<float> OnProgressChanged;
        public event Action<int> OnStarEarned;
    
        private LevelClearConditionConfig _levelClearConditionConfig;

        private int _starsEarned = 0;
    
        public int StarsEarned => _starsEarned;
    
        public LevelProgressTracker(BallCollisionProcessor ballCollisionProcessor, 
            GameplaySystemsManager gameplaySystemsManager,
            GameData gameData, 
            IConfigProvider configProvider)
        {
            _gameData = gameData;
            _configProvider = configProvider;

            gameplaySystemsManager.RegisterSystem(this);
            ballCollisionProcessor.OnBallsMatched += UpdateProgress;
        }

        public void Reset()
        {
            _starsEarned = 0;
            _levelClearConditionConfig ??= _configProvider.Get<LevelClearConditionConfig>();
        }

        private void UpdateProgress(HashSet<BallTarget> ballsMatched)
        {
            float originalProgress = _gameData.LevelProgress;

            int matchProgress = FindOriginalBallsMatched(ballsMatched);
            float newProgress = originalProgress + matchProgress * 1f / _gameData.LevelBallsCount;
        
            _gameData.LevelProgress = newProgress;
        
            OnProgressChanged?.Invoke(newProgress);
            NotifyStarsEarned(originalProgress, newProgress);
        }

        private int FindOriginalBallsMatched(HashSet<BallTarget> ballsMatched)
        {
            var originalBalls = _gameData.OriginalLevelBalls;
            return ballsMatched.Count(ball => originalBalls.Contains(ball));
        }

        private void NotifyStarsEarned(float origProg, float newProg)
        {
            if (origProg < _levelClearConditionConfig.OneStarProgressRequirement &&
                newProg >= _levelClearConditionConfig.OneStarProgressRequirement)
            {
                _starsEarned = 1;
                OnStarEarned?.Invoke(1);
            }

            if (origProg < _levelClearConditionConfig.TwoStarProgressRequirement &&
                newProg >= _levelClearConditionConfig.TwoStarProgressRequirement)
            {
                _starsEarned = 2;
                OnStarEarned?.Invoke(2);
            }

            if (origProg < _levelClearConditionConfig.ThreeStarProgressRequirement &&
                newProg >= _levelClearConditionConfig.ThreeStarProgressRequirement)
            {
                _starsEarned = 3;
                OnStarEarned?.Invoke(3);
            }
        }
    }
}

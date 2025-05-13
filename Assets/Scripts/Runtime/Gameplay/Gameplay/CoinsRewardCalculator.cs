using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay
{
    public class CoinsRewardCalculator
    {
        private const int BaseReward = 50;
        private const int RewardPerLevel = 5;
        private const float MultiplierPerStar = 0.33f;
    
        private readonly GameData _gameData;
        private readonly LevelProgressTracker _levelProgressTracker;

        public CoinsRewardCalculator(GameData gameData, LevelProgressTracker levelProgressTracker)
        {
            _gameData = gameData;
            _levelProgressTracker = levelProgressTracker;
        }
    
        public int CalculateReward()
        {
            int level = _gameData.LevelID + 1;
            int stars = _levelProgressTracker.StarsEarned;
        
            int levelReward = level * RewardPerLevel;

            return Mathf.RoundToInt((BaseReward + levelReward) * (1 + MultiplierPerStar * stars));
        }
    }
}

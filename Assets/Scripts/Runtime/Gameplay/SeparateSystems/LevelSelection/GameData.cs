using System;
using System.Collections.Generic;
using Runtime.Gameplay.Gameplay.Balls;

namespace Runtime.Gameplay.SeparateSystems.LevelSelection
{
    public class GameData
    {
        private int _launchedBalls;
        public event Action<int> OnBallLaunched;

        public int LevelID { get; set; } = 0;
        public float LevelProgress { get; set; } = 0;

        public int LaunchedBalls
        {
            get => _launchedBalls;
            set
            {
                _launchedBalls = value;
                OnBallLaunched?.Invoke(value);
            }
        }

        public int MaxLaunchedBalls { get; set; } = 0;

        public int LevelBallsCount { get; set; } = 0;
        public HashSet<BallTarget> ActiveBalls { get; set; }
        public HashSet<BallTarget> OriginalLevelBalls { get; set; }
    }
}
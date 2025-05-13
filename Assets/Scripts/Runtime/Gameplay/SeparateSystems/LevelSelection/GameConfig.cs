using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Runtime.Gameplay.SeparateSystems.LevelSelection
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
    public class GameConfig : BaseConfig
    {
        [SerializeField] private List<LevelConfig> _levelConfigs;

        public List<LevelConfig> LevelConfigs => _levelConfigs;
    }
}

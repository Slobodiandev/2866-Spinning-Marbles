using Core;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "LevelClearConditionConfig", menuName = "Config/LevelClearConditionConfig")]
    public class LevelClearConditionConfig : BaseConfig
    {
        public float OneStarProgressRequirement;
        public float TwoStarProgressRequirement;
        public float ThreeStarProgressRequirement;
    }
}

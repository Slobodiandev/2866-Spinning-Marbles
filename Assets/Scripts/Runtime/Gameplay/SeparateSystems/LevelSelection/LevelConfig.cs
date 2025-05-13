using UnityEngine;

namespace Runtime.Gameplay.SeparateSystems.LevelSelection
{
    [CreateAssetMenu(fileName = "Level Config 0", menuName = "Config/Level Config")]
    public class LevelConfig : ScriptableObject
    {
        public GameObject LevelPrefab;
        public GameObject AdditonalPrefab;
        public int BallsCount = 20;
    }
}

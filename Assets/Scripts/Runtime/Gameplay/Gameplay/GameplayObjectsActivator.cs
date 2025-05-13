using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay
{
    public class GameplayObjectsActivator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _gameplayObjects;
    
        private void EnableMenuObjects(bool enable) => _gameplayObjects.ForEach(obj => obj.SetActive(enable));
    
        public void Enable(bool enable)
        {
            EnableMenuObjects(enable);
        }
    }
}

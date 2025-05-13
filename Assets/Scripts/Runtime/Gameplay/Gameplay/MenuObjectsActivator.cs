using System.Collections.Generic;
using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay
{
    public class MenuObjectsActivator : MonoBehaviour, IEnableable 
    {
        [SerializeField] private List<GameObject> _menuObjects;

        [Inject]
        private void Construct(MenuSystemsManager manager)
        {
            manager.RegisterSystem(this);
        }
    
        private void EnableMenuObjects(bool enable) => _menuObjects.ForEach(obj => obj.SetActive(enable));
    
        public void Enable(bool enable)
        {
            EnableMenuObjects(enable);
        }
    }
}

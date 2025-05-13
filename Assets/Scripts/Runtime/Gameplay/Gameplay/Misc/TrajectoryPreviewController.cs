using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Gameplay.Systems;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay.Misc
{
    public class TrajectoryPreviewController : MonoBehaviour, IEnableable
    {
        [SerializeField] private Transform _trajectoryTransform;
    
        private PlayerInputProvider _playerInputProvider;
        private bool _enabled = false;
    
        [Inject]
        private void Construct(PlayerInputProvider playerInputProvider, GameplaySystemsManager gameplaySystemsManager)
        {
            _playerInputProvider = playerInputProvider;
            gameplaySystemsManager.RegisterSystem(this);
        }

        private void Update()
        {
            if(!_enabled)
                return;
        
            if (!_playerInputProvider.AnyInput)
            {
                _trajectoryTransform.gameObject.SetActive(false);
                return;
            }
        
            _trajectoryTransform.gameObject.SetActive(true);
            transform.up = _playerInputProvider.TouchWorldPosition - transform.position;
        }

        public void Enable(bool enable)
        {
            _enabled = enable;
        
            if(!_enabled)
                _trajectoryTransform.gameObject.SetActive(false);
        }
    }
}

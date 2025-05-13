using System;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Tools;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay.Systems
{
    public class PlayerInputProvider : BaseSystem, ITickable
    {
        private Camera _camera;
    
        private bool _anyInput;
    
        private Vector3 _lastTouchWorldPosition;
        private Vector3 _initialTouchWorldPosition;
        private Vector3 _lastTouchPosition;
        private Vector3 _initialTouchPosition;

        public bool AnyInput => _anyInput;
    
        public Vector3 InitialTouchWorldPosition => _initialTouchPosition;
        public Vector3 TouchWorldPosition => _lastTouchWorldPosition;

        public event Action OnInputEnded;

        [Inject]
        private void Construct(GameplaySystemsManager gameplaySystemsManager)
        {
            gameplaySystemsManager.RegisterSystem(this);
        }

        public override void Reset()
        {
            if(_camera == null)
                _camera = Camera.main;
        
            _anyInput = false;
            _lastTouchWorldPosition = 
                _initialTouchWorldPosition = 
                    _lastTouchPosition = 
                        _initialTouchPosition = Vector3.zero;
        }
    
        public void Tick()
        {
            if(!Enabled || Helper.IsPointerOverUIElement() || !CheckInput())
                return;
        
            Touch touch = Input.GetTouch(0);
        
            RecordFirstTouchPosition(touch);
            UpdateTouchPosition(touch);
        
            if(touch.phase == TouchPhase.Ended)
                OnInputEnded?.Invoke();
        }

        private void RecordFirstTouchPosition(Touch touch)
        {
            if (BeganTouch())
            {
                _initialTouchPosition = touch.position;
                UpdateTouchWorldPos(_initialTouchPosition, out _initialTouchWorldPosition);
            }
        }

        private void UpdateTouchPosition(Touch touch)
        {
            _lastTouchPosition = touch.position;
            UpdateLastTouchWorldPos();
        }

        private bool CheckInput()
        {
            _anyInput = Input.touchCount > 0;
            return _anyInput;
        }

        private bool BeganTouch() => Input.GetTouch(0).phase == TouchPhase.Began;

        private void UpdateLastTouchWorldPos() => UpdateTouchWorldPos(_lastTouchPosition, out _lastTouchWorldPosition);

        private void UpdateTouchWorldPos(Vector3 touchPos, out Vector3 worldPos)
        {
            worldPos = _camera.ScreenToWorldPoint(touchPos);
            worldPos.z = 0;
        }
    }
}

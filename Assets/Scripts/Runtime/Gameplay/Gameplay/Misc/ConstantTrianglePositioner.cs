using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Misc
{
    public class ConstantTrianglePositioner : MonoBehaviour
    {
        private Vector3 _defaultPosition;
    
        void Start()
        {
            _defaultPosition = transform.position;
        }

        void Update()
        {
            transform.position = _defaultPosition;
        }
    }
}

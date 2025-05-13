using UnityEngine;

namespace Runtime.Gameplay.UI
{
    public class MenuTriangleRotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        void Update()
        {
            transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
        }
    }
}

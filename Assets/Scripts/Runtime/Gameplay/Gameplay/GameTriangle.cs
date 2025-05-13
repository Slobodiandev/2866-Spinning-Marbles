using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay
{
    public class GameTriangle : MonoBehaviour, IResettable
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GameObject[] _stars;
    
        [Inject]
        private void Construct(GameplaySystemsManager gameplaySystemsManager)
        {
            gameplaySystemsManager.RegisterSystem(this);
        }

        public void Reset()
        {
            transform.rotation = Quaternion.identity;
            _rigidbody2D.velocity = Vector3.zero;
            _rigidbody2D.angularVelocity = 0;

            for (int i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive(false);
                _stars[i].transform.localScale = Vector3.zero;
            }
        }
    }
}

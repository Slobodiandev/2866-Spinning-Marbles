using DG.Tweening;
using Runtime.Gameplay.Gameplay.Misc;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay.Visuals
{
    public class GameTriangleProgress : MonoBehaviour
    {
        [SerializeField] private float _scaleAnimDuration;
        [SerializeField] private Transform[] _stars;
        [SerializeField] private float _rotateSpeed;
    
        [Inject]
        private void Construct(LevelProgressTracker levelProgressTracker)
        {
            levelProgressTracker.OnStarEarned += EnableStar; 
        }

        private void EnableStar(int amount)
        {
            _stars[amount - 1].gameObject.SetActive(true);
            _stars[amount - 1].DOScale(1, _scaleAnimDuration).SetEase(Ease.OutBounce).SetLink(gameObject);
        }

        private void Update()
        {
            float rotateAmount = _rotateSpeed * Time.deltaTime;
            foreach (var star in _stars)
            {
                star.Rotate(Vector3.forward, rotateAmount);
            }
        }
    }
}

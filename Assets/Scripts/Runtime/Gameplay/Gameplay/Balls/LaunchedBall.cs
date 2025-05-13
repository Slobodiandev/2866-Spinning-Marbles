using System;
using Core.Services.Audio;
using Runtime.Gameplay.Gameplay.Systems;
using Runtime.Gameplay.Gameplay.Visuals;
using Runtime.Gameplay.Services.Audio;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay.Balls
{
    public class LaunchedBall : BallTarget
    {
        private const int WallCollisionsToGetDestroyed = 5;

        [SerializeField] private Rigidbody2D _rigidbody2D;
    
        private float _velocity;
        private int _wallCollisions = 0;
    
        public static event Action<Collision2D, LaunchedBall> OnCollision;
        public event Action OnBallNotHitTriangle;
    
        private bool _stuck = false;
    
        private BallCollisionProcessor _ballCollisionProcessor;
        private BallsDestructionController _ballDestructionController;
        private IAudioService _audioService;
    
        [Inject]
        private void Construct(BallCollisionProcessor ballCollisionProcessor, BallsDestructionController ballsDestructionController, IAudioService audioService)
        {
            _ballCollisionProcessor = ballCollisionProcessor;
            _ballDestructionController = ballsDestructionController;
            _audioService = audioService;
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            if(_stuck)
                return;
        
            ProcessFirstCollision(other);
        }

        private void FixedUpdate()
        {
            if(_stuck)
                return;
        
            if (_rigidbody2D.velocity.sqrMagnitude > 0.01f)
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _velocity;
        }

        protected void ProcessFirstCollision(Collision2D other)
        {
            if (other.gameObject.CompareTag(ConstTags.Triangle))
            {
                FindNearestBall();
                OnCollision?.Invoke(other, this);
                return;
            }
        
            if (other.gameObject.CompareTag(ConstTags.WallCollider))
            {
                ProcessWallCollision();
            }
        }

        private void FindNearestBall()
        {
            (int count, Collider2D[] colliders) = _ballCollisionProcessor.GetCollidersInRadius(this);

            if (count <= 1)
                return;

            BallTarget nearestBall = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                if (colliders[i].TryGetComponent(out BallTarget otherBall) && otherBall != this)
                {
                    float distance = Vector3.Distance(otherBall.transform.position, transform.position);
                    if (distance < minDistance)
                    {
                        nearestBall = otherBall;
                        minDistance = distance;
                    }
                }
            }

            if (nearestBall != null)
            {
                ProcessNearestBall(nearestBall);
            }
        }

        protected virtual void ProcessNearestBall(BallTarget nearestBall)
        {
            if (nearestBall is RockBall rockBall)
                rockBall.BreakRock();
        }

        public void SetForce(Vector2 direction, float amount)
        {
            _collider2D.enabled = true;
            _velocity = amount;
            _rigidbody2D.AddForce(direction * _velocity, ForceMode2D.Impulse);
            _audioService.PlaySound(ConstAudio.ShootSound);
        }

        public void DisableRigidbody()
        {
            _stuck = true;
            Destroy(_rigidbody2D);
            OnBallNotHitTriangle?.Invoke();
        }
    
        private void ProcessWallCollision()
        {
            _wallCollisions++;

            if (_wallCollisions >= WallCollisionsToGetDestroyed)
            {
                _audioService.PlaySound(ConstAudio.DestructionSound);
                _ballDestructionController.DestroyBall(this);
                OnBallNotHitTriangle?.Invoke();
            }
            else
                _audioService.PlaySound(ConstAudio.CollisionSound);
        }
    }
}

using System;
using System.Collections.Generic;
using Core;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Configs;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay.Systems
{
    public class BallCollisionProcessor : BaseSystem
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameData _gameData;

        private CollisionProcessorConfig _config;
        private BallMatchFinder _matchFinder;
        private FloatingBallDetector _floatingDetector;

        public event Action<HashSet<BallTarget>> OnBallsMatched;
        public event Action<BallTarget> OnTriangleHit;
        public BallCollisionProcessor(IConfigProvider configProvider, GameData gameData)
        {
            _configProvider = configProvider;
            _gameData = gameData;
        }

        [Inject]
        private void Construct(GameplaySystemsManager gameplaySystemsManager)
        {
            gameplaySystemsManager.RegisterSystem(this);
        }

        public override void Reset()
        {
            _config ??= _configProvider.Get<CollisionProcessorConfig>();
            _matchFinder ??= new BallMatchFinder(_config);
            _floatingDetector ??= new FloatingBallDetector(_config, _gameData);
        }

        public override void Enable(bool enable)
        {
            base.Enable(enable);
        
            if (enable)
                LaunchedBall.OnCollision += ProcessBallCollision;
            else
                LaunchedBall.OnCollision -= ProcessBallCollision;
        }

        public (int, Collider2D[]) GetCollidersInRadius(LaunchedBall ball)
        {
            Collider2D[] hitBuffer = new Collider2D[_config.CollisionArraySize];
            float searchRadius = ball.Radius * _config.SearchRadiusMultiplier;
        
            int count = Physics2D.OverlapCircle(ball.transform.position, searchRadius, _config.AllCollisionFilter, hitBuffer);
            return (count, hitBuffer);
        }

        private void ProcessBallCollision(Collision2D collision, LaunchedBall ball)
        {
            float searchRadius = ball.Radius * _config.SearchRadiusMultiplier;
            Vector3 collisionPoint = collision.GetContact(0).point;

            if (HitTriangle(collisionPoint, ball.Radius * _config.TriangleHitSearchRadiusMultiplier))
            {
                OnTriangleHit?.Invoke(ball);
                return;
            }
        
            var matchedBalls = _matchFinder.FindMatches(ball, collisionPoint);
            if (matchedBalls.Count >= _config.MinMatchCount)
                HandleMatch(collision.transform, matchedBalls, searchRadius);
            else
            {
                _gameData.ActiveBalls.Add(ball);
                ball.transform.SetParent(collision.transform);
            }
        
            ball.DisableRigidbody();
        }
    
        private bool HitTriangle(Vector3 position, float radius)
        {
            Collider2D[] hitBuffer = new Collider2D[_config.CollisionArraySize];
            int count = Physics2D.OverlapCircle(position, radius, _config.AllCollisionFilter, hitBuffer);

            for (int i = 0; i < count; i++)
            {
                if (hitBuffer[i].gameObject.CompareTag(ConstTags.Triangle))
                    return true;
            }
            return false;
        }

        private void HandleMatch(Transform triangle, HashSet<BallTarget> matchedBalls, float searchRadius)
        {
            HashSet<BallTarget> ballsToDestroy = new(matchedBalls.Count * 2);
    
            foreach (BallTarget ball in matchedBalls)
            {
                _gameData.ActiveBalls.Remove(ball);
                ballsToDestroy.Add(ball);
            }

        
            List<BallTarget> floatingBalls = _floatingDetector.FindFloatingBalls(searchRadius, triangle, ballsToDestroy);

            foreach (BallTarget ball in floatingBalls)
            {
                _gameData.ActiveBalls.Remove(ball);
                ballsToDestroy.Add(ball);
            }
    
            OnBallsMatched?.Invoke(ballsToDestroy);
        }
    }
}
using System.Collections.Generic;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Configs;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Systems
{
    public class BallMatchFinder
    {
        private readonly CollisionProcessorConfig _config;

        public BallMatchFinder(CollisionProcessorConfig config)
        {
            _config = config;
        }

        public HashSet<BallTarget> FindMatches(BallTarget startBall, Vector3 startPosition)
        {
            HashSet<BallTarget> matchedBalls = new() { startBall };
            Queue<BallTarget> queue = new();
            Collider2D[] hitBuffer = new Collider2D[_config.CollisionArraySize];

            int count = Physics2D.OverlapCircle(startPosition, startBall.Radius * _config.SearchRadiusMultiplier, _config.BallFilter, hitBuffer);
            ProcessNeighbors(hitBuffer, count, startBall.GetBallTypeID(), matchedBalls, queue);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                count = Physics2D.OverlapCircle(current.transform.position, startBall.Radius * _config.SearchRadiusMultiplier, _config.BallFilter, hitBuffer);
                ProcessNeighbors(hitBuffer, count, startBall.GetBallTypeID(), matchedBalls, queue);
            }

            return matchedBalls;
        }

        private void ProcessNeighbors(Collider2D[] hitBuffer, int count, int ballTypeId, HashSet<BallTarget> matchedBalls, Queue<BallTarget> queue)
        {
            for (int i = 0; i < count; i++)
            {
                BallTarget ball = hitBuffer[i].GetComponent<BallTarget>();
                if (ball != null && ball.GetBallTypeID() == ballTypeId && matchedBalls.Add(ball))
                    queue.Enqueue(ball);
            }
        }
    }
}
using System.Collections.Generic;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Configs;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Systems
{
    public class FloatingBallDetector
    {
        private readonly CollisionProcessorConfig _config;
        private readonly GameData _gameData;

        public FloatingBallDetector(CollisionProcessorConfig config, GameData gameData)
        {
            _config = config;
            _gameData = gameData;
        }

        public List<BallTarget> FindFloatingBalls(float searchRadius, Transform triangle, HashSet<BallTarget> ballsToBeDestroyed)
        {
            List<BallTarget> floatingBalls = new(_gameData.ActiveBalls.Count / 2);
            HashSet<BallTarget> visited = new(_gameData.ActiveBalls.Count);

            foreach (BallTarget ball in _gameData.ActiveBalls)
            {
                if (visited.Contains(ball) || ballsToBeDestroyed.Contains(ball))
                    continue;

                (HashSet<BallTarget> group, bool isConnected) = GetConnectedGroup(ball, searchRadius, triangle, visited, ballsToBeDestroyed);
                if (!isConnected)
                    floatingBalls.AddRange(group);
            }

            return floatingBalls;
        }

        private (HashSet<BallTarget> group, bool isConnected) GetConnectedGroup(
            BallTarget startBall, 
            float searchRadius, 
            Transform triangle, 
            HashSet<BallTarget> visited, 
            HashSet<BallTarget> ballsToBeDestroyed)
        {
            HashSet<BallTarget> group = new();
            Queue<BallTarget> queue = new Queue<BallTarget>();
            Collider2D[] hitBuffer = new Collider2D[_config.CollisionArraySize];

            queue.Enqueue(startBall);
            group.Add(startBall);
            visited.Add(startBall);

            bool connectedToTriangle = false;
        
            while (queue.Count > 0)
            {
                BallTarget current = queue.Dequeue();
            
                if(current == null)
                    continue;
            
                int count = Physics2D.OverlapCircle(current.transform.position, searchRadius, _config.AllCollisionFilter, hitBuffer);

                for (int i = 0; i < count; i++)
                {
                    Collider2D collider = hitBuffer[i];

                    if (collider.transform == triangle)
                        connectedToTriangle = true;

                    BallTarget neighbor = collider.GetComponent<BallTarget>();
                    if (neighbor != null 
                        && !visited.Contains(neighbor) 
                        && !ballsToBeDestroyed.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        group.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return (group, connectedToTriangle);
        }
    }
}

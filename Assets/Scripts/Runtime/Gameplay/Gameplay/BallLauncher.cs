using System;
using System.Collections;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Gameplay.Systems;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay
{
    public class BallLauncher : MonoBehaviour, IResettable
    {
        [SerializeField] private float _shootCD = 0.5f;
        [SerializeField] private float _launchForce;
        [SerializeField] private float _queueCooldown;
    
        private PlayerInputProvider _playerInputProvider;
        private LaunchedBallsFactory _factory;
        private GameData _gameData;

        private LaunchedBall _ball;
        private bool _canShoot = true;
        private bool _shootInQueue = false;
    
        public event Action<LaunchedBall> OnLastBallLaunched;
    
        [Inject]
        private void Construct(PlayerInputProvider playerInputProvider, LaunchedBallsFactory factory, 
            GameplaySystemsManager gameplaySystemsManager, GameData gameData)
        {
            _playerInputProvider = playerInputProvider;
            _factory = factory;
            _gameData = gameData;
        
            gameplaySystemsManager.RegisterSystem(this);
        
            _playerInputProvider.OnInputEnded += LaunchBall;
        }
    
        public void Reset()
        {
            StopAllCoroutines();
            _canShoot = true;
            _shootInQueue = false;
        
            _ball = _factory.CreateDefaultBall();
            _ball.transform.position = transform.position;
        }

        public void EnableQueue()
        {
            if(!CanRequestBall())
                return;
        
            _shootInQueue = true;
        }

        public void SetGhostBall()
        {
            if(!CanRequestBall())
                return;
        
            ChangeBall(_factory.CreateGhostBall());
        }

        public void SetMultiColorBall()
        {
            if(!CanRequestBall())
                return;
        
            ChangeBall(_factory.CreateMultiColorBall());
        }

        private void ChangeBall(LaunchedBall newBall)
        {
            if(_ball != null)
                Destroy(_ball.gameObject);

            _ball = newBall;
            _ball.transform.position = transform.position;
            StopAllCoroutines();
        }

        private void LaunchBall()
        {
            if(!_canShoot)
                return;

            if(_shootInQueue)
                StartCoroutine(ShootInQueue());
            else
                StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            _canShoot = false;
            ShootBall(_ball);
            yield return WaitForCooldown(_shootCD);
            GetNextDefaultBall();
        }

        private IEnumerator ShootInQueue()
        {
            _canShoot = false;
            _shootInQueue = false;
        
            var nextBall = _factory.CreateBallCopy(_ball);
            nextBall.gameObject.SetActive(false);
        
            ShootBall(_ball);
        
            yield return WaitForCooldown(_queueCooldown);

            ActivateAndPositionBall(nextBall);
            ShootBall(nextBall);

            yield return WaitForCooldown(_shootCD);
            GetNextDefaultBall();
        }

        private void ShootBall(LaunchedBall ball)
        {
            Vector3 launchDirection = _playerInputProvider.TouchWorldPosition - transform.position;
            ball.SetForce(launchDirection.normalized, _launchForce);
        
            if(_gameData.LaunchedBalls == _gameData.MaxLaunchedBalls)
                OnLastBallLaunched?.Invoke(ball);
        }

        private void GetNextDefaultBall()
        {
            if(!CanRequestBall())
                return;
        
            _canShoot = true;
            _ball = _factory.CreateDefaultBall();
            _ball.transform.position = transform.position;
        }

        private void ActivateAndPositionBall(LaunchedBall ball)
        {
            ball.gameObject.SetActive(true);
            ball.transform.position = transform.position;
        }

        private IEnumerator WaitForCooldown(float cooldown)
        {
            float elapsed = 0;
            while (elapsed < cooldown)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        private bool CanRequestBall() =>_gameData.LaunchedBalls < _gameData.MaxLaunchedBalls;
    }
}

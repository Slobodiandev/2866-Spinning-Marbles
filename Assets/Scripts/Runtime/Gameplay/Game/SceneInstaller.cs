using Runtime.Gameplay.Gameplay;
using Runtime.Gameplay.Gameplay.Visuals;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Game
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private BallLauncher _ballLauncher;
        [SerializeField] private GameEndAnimationController _animationController;
        [SerializeField] private GameplayObjectsActivator _gameplayObjectsActivator;
        [SerializeField] private GameTriangle _gameTriangle;
    
        public override void InstallBindings()
        {
            Container.Bind<BallLauncher>().FromInstance(_ballLauncher).AsSingle();
            Container.Bind<GameEndAnimationController>().FromInstance(_animationController).AsSingle();
            Container.Bind<GameplayObjectsActivator>().FromInstance(_gameplayObjectsActivator).AsSingle();
            Container.Bind<GameTriangle>().FromInstance(_gameTriangle).AsSingle();
        }
    }
}

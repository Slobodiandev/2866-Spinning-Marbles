using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game;
using Zenject;

namespace Runtime.Gameplay.Initialization
{
    public class Bootstrapper : IInitializable
    {
        private readonly StateMachine _stateMachine;
        private readonly BootstrapStateMachine _bootstrapStateMachine;
        private readonly GameStateMachine _gameStateMachine;

        public Bootstrapper(StateMachine stateMachine, BootstrapStateMachine bootstrapStateMachine, GameStateMachine gameStateMachine)
        {
            _stateMachine = stateMachine;
            _bootstrapStateMachine = bootstrapStateMachine;
            _gameStateMachine = gameStateMachine;
        }

        public void Initialize()
        {
            _stateMachine.InitializeStateMachine(_bootstrapStateMachine, _gameStateMachine);
            _stateMachine.GoToState<BootstrapStateMachine>().Forget();
            UnityEngine.Application.targetFrameRate = 60;
        }
    }
}
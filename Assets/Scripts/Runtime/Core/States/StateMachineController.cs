using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public abstract class StateMachineController
    {
        private StateMachine _stateMachine;

        protected readonly ICustomLogger CustomLogger;

        protected StateMachineController(ICustomLogger customLogger)
        {
            CustomLogger = customLogger;
        }

        public void InitializeMachine(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public abstract UniTask EnterState(CancellationToken cancellationToken = default);

        public virtual UniTask ExitState()
        {
            return UniTask.CompletedTask;
        }

        protected async UniTask GoToState<T>(CancellationToken cancellationToken = default) where T : StateMachineController
        {
            await _stateMachine.GoToState<T>(cancellationToken);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.StateMachine
{
    public class StateMachine
    {
        private Dictionary<Type, StateMachineController> _states;
        private StateMachineController _activeStateMachine;
        private bool IsActive => _activeStateMachine != null;

        public void InitializeStateMachine(params StateMachineController[] stateControllers)
        {
            _states = new Dictionary<Type, StateMachineController>(stateControllers.Length);
            foreach (var executiveStateController in stateControllers)
            {
                _states.Add(executiveStateController.GetType(), executiveStateController);
                executiveStateController.InitializeMachine(this);
            }
        }

        public async UniTask GoToState<TState>(CancellationToken cancellationToken = default) where TState : StateMachineController
        {
            StateMachineController stateMachine = await ChangeState<TState>();
            await stateMachine.EnterState(cancellationToken);
        }

        private async UniTask<TState> ChangeState<TState>() where TState : StateMachineController
        {
            if (_activeStateMachine != null)
                await _activeStateMachine.ExitState();

            TState state = GetState<TState>();
            _activeStateMachine = state;

            return state;
        }

        private TState GetState<TState>() where TState : class
        {
            return _states[typeof(TState)] as TState;
        }
    }
}
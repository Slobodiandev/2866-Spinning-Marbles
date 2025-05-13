using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace Core
{
    public abstract class BaseController
    {
        protected ControllerStates CurrentStates = ControllerStates.Pending;

        public ControllerStates CurrentControllerStates => CurrentStates;

        public virtual UniTask RunController(CancellationToken cancellationToken)
        {
            Assert.IsFalse(CurrentStates == ControllerStates.Run, $"{this.GetType().Name}: try run already running controller");

            CurrentStates = ControllerStates.Run;
            return UniTask.CompletedTask;
        }

        public virtual UniTask StopController()
        {
            Assert.IsFalse(CurrentStates != ControllerStates.Run, $"{this.GetType().Name}: try to stop not active controller");

            CurrentStates = ControllerStates.Stop;
            return UniTask.CompletedTask;
        }
    }

    public abstract class BaseController<T> : BaseController where T : class
    {
        public sealed override UniTask RunController(CancellationToken cancellationToken)
        {
            return base.RunController(cancellationToken);
        }

        public virtual UniTask Run(T data, CancellationToken cancellationToken)
        {
            base.RunController(cancellationToken);
            return UniTask.CompletedTask;
        }
    }
}
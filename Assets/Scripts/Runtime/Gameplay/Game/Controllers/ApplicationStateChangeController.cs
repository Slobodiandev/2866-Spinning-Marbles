using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.ApplicationState;
using Runtime.Gameplay.Services.UserData;

namespace Runtime.Gameplay.Game.Controllers
{
    public class ApplicationStateChangeController : BaseController
    {
        private readonly ApplicationStateService _applicationStateService;
        private readonly UserDataService _userDataService;

        public ApplicationStateChangeController(ApplicationStateService applicationStateService,
            UserDataService userDataService)
        {
            _applicationStateService = applicationStateService;
            _userDataService = userDataService;
        }

        public override UniTask RunController(CancellationToken cancellationToken)
        {
            base.RunController(cancellationToken);

            _applicationStateService.Initialize();

            _applicationStateService.ApplicationQuitEvent += SaveUserData;
            _applicationStateService.ApplicationPauseEvent += SaveIfPaused;

            return UniTask.CompletedTask;
        }

        public override UniTask StopController()
        {
            base.StopController();

            _applicationStateService.ApplicationQuitEvent -= SaveUserData;
            _applicationStateService.ApplicationPauseEvent -= SaveIfPaused;

            _applicationStateService.Dispose();

            return UniTask.CompletedTask;
        }

        private void SaveUserData()
        {
            _userDataService.SaveUserData();
        }

        private void SaveIfPaused(bool isPause)
        {
            if (isPause)
                _userDataService.SaveUserData();
        }
    }
}
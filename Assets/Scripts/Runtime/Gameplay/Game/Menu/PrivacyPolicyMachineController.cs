using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class PrivacyPolicyMachineController : StateMachineController
    {
        private readonly IUiService _uiService;

        private PrivacyPolicyScreen _screen;

        public PrivacyPolicyMachineController(ICustomLogger customLogger, IUiService uiService) : base(customLogger)
        {
            _uiService = uiService;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.PrivacyPolicyScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<PrivacyPolicyScreen>(ConstScreens.PrivacyPolicyScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoToState<MenuStateMachineController>();
        }
    }
}
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class TermsOfUseMachineController : StateMachineController
    {
        private readonly IUiService _uiService;

        private TermsOfUseScreen _screen;

        public TermsOfUseMachineController(ICustomLogger customLogger, IUiService uiService) : base(customLogger)
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
            await _uiService.HideScreen(ConstScreens.TermsOfUseScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<TermsOfUseScreen>(ConstScreens.TermsOfUseScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoToState<MenuStateMachineController>();
        }
    }
}
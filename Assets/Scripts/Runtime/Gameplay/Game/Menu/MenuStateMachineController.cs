using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class MenuStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly AccountScreenStateMachineController _accountScreenStateMachineController;

        private MenuScreen _screen;

        public MenuStateMachineController(ICustomLogger customLogger, IUiService uiService,
            AccountScreenStateMachineController accountScreenStateMachineController) : base(customLogger)
        {
            _uiService = uiService;
            _accountScreenStateMachineController = accountScreenStateMachineController;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.MenuScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<MenuScreen>(ConstScreens.MenuScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoToState<MainScreenMachineController>();
            _screen.OnInventoryPressed += async () => await GoToState<InventoryScreenMachineController>();
            _screen.OnPrivacyPressed += async () => await GoToState<PrivacyPolicyMachineController>();
            _screen.OnTouPressed += async () => await GoToState<TermsOfUseMachineController>();
            _screen.OnSettingsPressed += async () => await GoToState<SettingsScreenMachineController>();
            _screen.OnProfilePressed += async () =>
            {
                _accountScreenStateMachineController.SetLastScreen(AccountScreenStateMachineController.LastScreen.Menu);
                await GoToState<AccountScreenStateMachineController>();
            };
        }
    }
}
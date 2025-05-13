using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Controllers;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class MainScreenMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly MenuSystemsManager _menuSystemsManager;
        
        private MainScreen _screen;

        public MainScreenMachineController(ICustomLogger customLogger, IUiService uiService, MenuSystemsManager menuSystemsManager) : base(customLogger)
        {
            _uiService = uiService;
            _menuSystemsManager = menuSystemsManager;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            _menuSystemsManager.EnableAllSystems(true);
            
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            _menuSystemsManager.EnableAllSystems(false);
            await _uiService.HideScreen(ConstScreens.MainScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<MainScreen>(ConstScreens.MainScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnPlayPressed += async () => await GoToState<LevelSelectionStateMachineController>();
            _screen.OnDailyPressed += async () => await GoToState<DailyRewardScreenMachineController>();
            _screen.OnHelpPressed += async () => await _uiService.ShowWindow(ConstPopups.HowToPlayPopup);
            _screen.OnMenuPressed += async () => await GoToState<MenuStateMachineController>();
            _screen.OnShopPressed += async () => await GoToState<InitShopStateMachine>();
        }
    }
}
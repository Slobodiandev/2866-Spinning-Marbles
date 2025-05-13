using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class LevelSelectionStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly UserProgressService _userProgressService;
        private readonly GameData _gameData;

        private LevelSelectionScreen _screen;

        private int _selectedLevelID = 0;

        public LevelSelectionStateMachineController(ICustomLogger customLogger, IUiService uiService, 
            UserProgressService userProgressService, GameData gameData) : base(customLogger)
        {
            _uiService = uiService;
            _userProgressService = userProgressService;
            _gameData = gameData;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            UpdateLastUnlockedLevelID();
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }
        
        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.LevelSelectionScreen);
        }

        private void UpdateLastUnlockedLevelID()
        {
            _selectedLevelID = _userProgressService.GetLastUnlockedLevelID();
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<LevelSelectionScreen>(ConstScreens.LevelSelectionScreen);
            _screen.Initialize(_selectedLevelID, _userProgressService.GetLevelsClearData());
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoToState<MainScreenMachineController>();
            _screen.OnSelectedLevelChanged += (level) => _selectedLevelID = level;
            _screen.OnPlayPressed += async () =>
            {
                _gameData.LevelID = _selectedLevelID;
                await GoToState<GameplayScreenMachineController>();
            };
        }
    }
}
using System.Threading;
using Core;
using Core.Services.Audio;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Controllers;
using Runtime.Gameplay.Gameplay;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Gameplay.Systems;
using Runtime.Gameplay.Services.Audio;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class GameplayScreenMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly PausePopupStateMachineController _pausePopupStateMachineController;
        private readonly SettingsPopupStateController _settingsPopupStateController;
        private readonly GameSetupController _gameSetupController;
        private readonly BallCollisionProcessor _ballCollisionProcessor;
        private readonly GameResultMachineController _gameResultMachineController;
        private readonly IAudioService _audioService;
        private readonly BallLauncher _ballLauncher;
        private readonly LosePopupStateMachineController _losePopupStateMachineController;
        private readonly GameplaySystemsManager _gameplaySystemsManager;

        private GameplayScreen _screen;

        public GameplayScreenMachineController(ICustomLogger customLogger, IUiService uiService,
            PausePopupStateMachineController pausePopupStateMachineController, SettingsPopupStateController settingsPopupStateController,
            GameSetupController gameSetupController, BallCollisionProcessor ballCollisionProcessor,
            GameResultMachineController gameResultMachineController, IAudioService audioService, BallLauncher ballLauncher,
            LosePopupStateMachineController losePopupStateMachineController, GameplaySystemsManager gameplaySystemsManager) : base(customLogger)
        {
            _uiService = uiService;
            _pausePopupStateMachineController = pausePopupStateMachineController;
            _settingsPopupStateController = settingsPopupStateController;
            _gameSetupController = gameSetupController;
            _ballCollisionProcessor = ballCollisionProcessor;
            _gameResultMachineController = gameResultMachineController;
            _audioService = audioService;
            _ballLauncher = ballLauncher;
            _losePopupStateMachineController = losePopupStateMachineController;
            _gameplaySystemsManager = gameplaySystemsManager;
            
            _ballCollisionProcessor.OnTriangleHit += ProcessTriangleHit;
            _ballLauncher.OnLastBallLaunched += TrackLastBall;
        }

        private void ProcessTriangleHit(BallTarget _)
        {
            _audioService.PlaySound(ConstAudio.VictorySound);
            _gameSetupController.DisableGameSystems();
            _gameResultMachineController.EnterState().Forget();
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            _gameSetupController.SetupGame();
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            _gameSetupController.DisableGameSystems();
            _gameSetupController.DisableGameplayObjects();
            
            await _uiService.HideScreen(ConstScreens.GameplayScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<GameplayScreen>(ConstScreens.GameplayScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnSettingsPressed += () => _settingsPopupStateController.RunController(CancellationToken.None).Forget();
            _screen.OnPausePressed += () => _pausePopupStateMachineController.EnterState().Forget();
        }

        private void TrackLastBall(LaunchedBall ball)
        {
            _gameplaySystemsManager.EnableSystem<TrajectoryPreviewController>(false);
            ball.OnBallNotHitTriangle += () =>
            {
                _losePopupStateMachineController.EnterState().Forget();
            };
        }
    }
}
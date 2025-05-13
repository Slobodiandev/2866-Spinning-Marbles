using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game;
using Runtime.Gameplay.Initialization.Controllers;
using Runtime.Gameplay.Services.ScreenOrientation;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData;
using UnityEngine;

namespace Runtime.Gameplay.Initialization
{
    public class BootstrapStateMachine : StateMachineController
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUiService _uiService;
        private readonly IConfigProvider _configProvider;
        private readonly UserDataService _userDataService;
        private readonly AudioSettingsBootstrapController _audioSettingsBootstrapController;
        private readonly ScreenOrientationAlertController _screenOrientationAlertController;

        public BootstrapStateMachine(IAssetProvider assetProvider,
            IUiService uiService,
            ICustomLogger customLogger,
            IConfigProvider configProvider,
            UserDataService userDataService,
            AudioSettingsBootstrapController audioSettingsBootstrapController,
            ScreenOrientationAlertController screenOrientationAlertController) : base(customLogger)
        {
            _assetProvider = assetProvider;
            _uiService = uiService;
            _configProvider = configProvider;
            _userDataService = userDataService;
            _audioSettingsBootstrapController = audioSettingsBootstrapController;
            _screenOrientationAlertController = screenOrientationAlertController;
        }

        public override async UniTask EnterState(CancellationToken cancellationToken)
        {
            Input.multiTouchEnabled = false;

            _userDataService.Initialize();
            await _assetProvider.Initialize();
            await _uiService.Initialize();
            await _configProvider.Initialize();
            await _screenOrientationAlertController.RunController(CancellationToken.None);
            _uiService.ShowScreen(ConstScreens.SplashScreen, cancellationToken).Forget();
            await _audioSettingsBootstrapController.RunController(CancellationToken.None);
            IncreaseSessionNumber();

            GoToState<GameStateMachine>().Forget();
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.SplashScreen);
        }

        private void IncreaseSessionNumber()
        {
            _userDataService.RetrieveUserData().AppData.SessionNumber++;
            _userDataService.SaveUserData();
        }
    }
}
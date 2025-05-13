using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Services.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Runtime.Gameplay.Services.ScreenOrientation
{
    public class ScreenOrientationAlertController : BaseController, ITickable
    {
        private readonly IUiService _uiService;
        private readonly IConfigProvider _configProvider;

        private ScreenOrientationAlertWindow _alertWindow;
        private ScreenOrientationConfig _config;
        private bool _initialized;

        public ScreenOrientationAlertController(IUiService uiService, IConfigProvider configProvider)
        {
            _uiService = uiService;
            _configProvider = configProvider;
        }

        public override UniTask RunController(CancellationToken cancellationToken)
        {
            Init();
            
            return base.RunController(cancellationToken);
        }

        public void Tick()
        {
            if(!_initialized)
                return;

            if(!_config || !_config.EnableScreenOrientationPopup)
                return;

            CheckScreenOrientation();
        }

        private void CheckScreenOrientation()
        {
            var currentScreenMode = Screen.orientation;

            if(IsSameScreenMode(currentScreenMode))
            {
                if(!_alertWindow.gameObject.activeSelf)
                    return;

                _alertWindow.HideWindow();
                ApplicationPauseController.TakePause(GameStateTypeId.RunningState);

                return;
            }

            ApplicationPauseController.TakePause(GameStateTypeId.PausedState);

            if(!_alertWindow.gameObject.activeSelf)
            {
                HideKeyboard();
                _alertWindow.ShowWindow(null);
            }
        }

        private static void HideKeyboard() => EventSystem.current?.SetSelectedGameObject(null);

        private bool IsSameScreenMode(UnityEngine.ScreenOrientation currentScreenMode)
        {
            if(_config.ScreenOrientationTypes == ScreenOrientationTypes.Portrait)
            {
                if(currentScreenMode is UnityEngine.ScreenOrientation.Portrait or UnityEngine.ScreenOrientation.PortraitUpsideDown)
                {
                    return true;
                }
            }

            if(_config.ScreenOrientationTypes != ScreenOrientationTypes.Landscape)
                return (int)currentScreenMode == (int)_config.ScreenOrientationTypes;

            return currentScreenMode is UnityEngine.ScreenOrientation.LandscapeLeft or UnityEngine.ScreenOrientation.LandscapeRight;
        }

        private void Init()
        {
            _config = _configProvider.Get<ScreenOrientationConfig>();

            if(!_config || !_config.EnableScreenOrientationPopup)
                return;

            _alertWindow = _uiService.GetWindow<ScreenOrientationAlertWindow>(ConstPopups.ScreenOrientationAlertPopup);
            _alertWindow.HideWindow();

            _initialized = true;
        }
    }
}
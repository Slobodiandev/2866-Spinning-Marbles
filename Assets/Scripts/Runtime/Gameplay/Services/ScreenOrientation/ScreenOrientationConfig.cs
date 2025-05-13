using Core;
using UnityEngine;

namespace Runtime.Gameplay.Services.ScreenOrientation
{
    [CreateAssetMenu(fileName = "ScreenOrientationConfig", menuName = "Config/ScreenOrientationConfig")]
    public sealed class ScreenOrientationConfig : BaseConfig
    {
        public ScreenOrientationTypes ScreenOrientationTypes;
        public bool EnableScreenOrientationPopup;
    }
}
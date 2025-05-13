using System.Collections.Generic;
using Core.UI;
using Runtime.Gameplay.UI.Screen;
using UnityEngine;

namespace Runtime.Gameplay.Services.UI
{
    public sealed class UiServiceViewContainer : MonoBehaviour
    {
        [SerializeField] private ScreenFade _screenFade;
        [SerializeField] private List<UiScreen> _screensPrefab;
        [SerializeField] private List<BaseWindow> _popupsPrefab;
        [SerializeField] private Transform _screenParent;

        public ScreenFade ScreenFade => _screenFade;
        public List<UiScreen> ScreensPrefab => _screensPrefab;
        public List<BaseWindow> PopupsPrefab => _popupsPrefab;
        public Transform ScreenParent => _screenParent;
    }
}
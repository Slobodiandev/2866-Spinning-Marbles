using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Screen
{
    public class TermsOfUseScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        
        public event Action OnBackPressed;

        public void Initialize() => _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
    }
}
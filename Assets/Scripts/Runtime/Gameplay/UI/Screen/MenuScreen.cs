using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.UI.Screen
{
    public class MenuScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _profileButton;
        [SerializeField] private Button _touButton;
        [SerializeField] private Button _privacyButton;
        
        public event Action OnBackPressed;
        public event Action OnInventoryPressed;
        public event Action OnSettingsPressed;
        public event Action OnProfilePressed;
        public event Action OnTouPressed;
        public event Action OnPrivacyPressed;

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _inventoryButton.onClick.AddListener(() => OnInventoryPressed?.Invoke());
            _settingsButton.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _profileButton.onClick.AddListener(() => OnProfilePressed?.Invoke());
            _touButton.onClick.AddListener(() => OnTouPressed?.Invoke());
            _privacyButton.onClick.AddListener(() => OnPrivacyPressed?.Invoke());
        }
    }
}
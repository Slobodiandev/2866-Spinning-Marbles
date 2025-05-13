using System;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.UserData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Gameplay.UI.Screen
{
    public class MainScreen : UiScreen
    {
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _dailyButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _clearText;
        [SerializeField] private TextMeshProUGUI _balanceText;

        public event Action OnHelpPressed;
        public event Action OnMenuPressed;
        public event Action OnShopPressed;
        public event Action OnDailyPressed;
        public event Action OnPlayPressed;
        
        [Inject]
        private void Construct(UserProgressService userProgressService, IUserInventoryService userInventoryService)
        {
            _clearText.text = userProgressService.GetTotalStars().ToString();
            _balanceText.text = userInventoryService.GetBalance().ToString();
        }

        public void Initialize()
        {
            _helpButton.onClick.AddListener(() => OnHelpPressed?.Invoke());
            _menuButton.onClick.AddListener(() => OnMenuPressed?.Invoke());
            _shopButton.onClick.AddListener(() => OnShopPressed?.Invoke());
            _dailyButton.onClick.AddListener(() => OnDailyPressed?.Invoke());
            _playButton.onClick.AddListener(() => OnPlayPressed?.Invoke());
        }
    }
}
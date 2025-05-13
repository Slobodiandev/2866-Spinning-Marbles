using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Gameplay.DailyLogin;
using Runtime.Gameplay.Services.UserData.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Gameplay.UI.Screen
{
    public class DailyRewardScreen : UiScreen
    {
        private const float FadeDuration = 0.2f;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private Button _spinButton;
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private GameObject _errorGO;
        [SerializeField] private VerticalLayoutGroup[] _columns;
        [SerializeField] private Image _blackBG;
        
        private IUserInventoryService _userInventoryService;
        
        public VerticalLayoutGroup[] Columns => _columns;
        
        public event Action OnBackPressed;
        public event Action OnHelpPressed;
        public event Action OnSpinPressed;

        [Inject]
        private void Construct(IUserInventoryService userInventoryService)
        {
            _userInventoryService = userInventoryService;
            _balanceText.text = _userInventoryService.GetBalance().ToString();
            _userInventoryService.BalanceChangedEvent += UpdateBalance;
        }

        private void OnDestroy()
        {
            _userInventoryService.BalanceChangedEvent -= UpdateBalance;
        }

        private void UpdateBalance(int balance)
        {
            _balanceText.text = balance.ToString();
        }

        public void Initialize()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _helpButton.onClick.AddListener(() => OnHelpPressed?.Invoke());
            _spinButton.onClick.AddListener(() => OnSpinPressed?.Invoke());
        }

        public void ParentRouletteItems(List<RouletteItemView> items, int columnID)
        {
            foreach (var item in items)
            {
                item.transform.SetParent(_columns[columnID].transform, false);
            }
        }
        
        public void DisableRoulette()
        {
            _errorGO.SetActive(true);
            _spinButton.interactable = false;
        }

        public void EnableFlowComponents(bool enable)
        {
            if (enable)
            {
                _blackBG.DOFade(0, FadeDuration).OnComplete(() =>
                {
                    _blackBG.gameObject.SetActive(false);
                    _spinButton.interactable = true;
                });
            }
            else
            {
                _spinButton.interactable = false;
                _blackBG.gameObject.SetActive(true);
                _blackBG.DOFade(0.95f, FadeDuration);
            }
        }
    }
}
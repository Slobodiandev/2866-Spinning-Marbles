using System;
using System.Collections.Generic;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Gameplay.UI.Screen
{
    public class LevelSelectionScreen : UiScreen
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _clearText;

        [SerializeField, Space] private RectTransform _levelSelectionButtonsParent;

        [SerializeField, Space] private LevelSelectionButtonStatusDisplay _selectedButtonDisplay;
        [SerializeField] private LevelSelectionButtonStatusDisplay _unlockedButtonDisplay;
        [SerializeField] private LevelSelectionButtonStatusDisplay _lockedButtonDisplay;

        [SerializeField] private Sprite _activeStar;
        
        public event Action OnBackPressed;
        public event Action OnPlayPressed;
        public event Action<int> OnSelectedLevelChanged;

        private int _lastSelectedButtonID = 0;

        private LevelSelectionButton[] _levelSelectionButtons;

        [Inject]
        private void Construct(UserProgressService userProgressService)
        {
            _clearText.text = userProgressService.GetTotalStars().ToString();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        public void Initialize(int lastUnlockedID, List<int> stars)
        {
            SubscribeToEvents();
            FindLevelSelectionButtons();
            InitializeButtons(lastUnlockedID, stars);
        }

        private void SubscribeToEvents()
        {
            _backButton.onClick.AddListener(() => OnBackPressed?.Invoke());
            _playButton.onClick.AddListener(() => OnPlayPressed?.Invoke());
        }

        private void UnsubscribeFromEvents()
        {
            _backButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();

            int size = _levelSelectionButtons.Length;
            for (int i = 0; i < size; i++)
                _levelSelectionButtons[i].OnLevelSelected -= UpdateSelectedLevel;
        }

        private void FindLevelSelectionButtons()
        {
            _levelSelectionButtons = _levelSelectionButtonsParent.GetComponentsInChildren<LevelSelectionButton>();
        }

        private void InitializeButtons(int lastUnlockedLevelID, List<int> stars)
        {
            int size = _levelSelectionButtons.Length;

            _lastSelectedButtonID = lastUnlockedLevelID;

            for (int i = 0; i < size; i++)
            {
                bool locked = i > lastUnlockedLevelID;
                bool selected = i == lastUnlockedLevelID;

                var button = _levelSelectionButtons[i];
                InitializeButton(locked, selected, button, GetStarsAmount(i, stars));
                button.OnLevelSelected += UpdateSelectedLevel;
            }
        }

        private int GetStarsAmount(int currentLevelID, List<int> stars)
        {
            if (currentLevelID >= stars.Count)
                return 0;
            
            return stars[currentLevelID];
        }

        private void InitializeButton(bool locked, bool selected, LevelSelectionButton button, int stars)
        {
            button.Initialize(locked, stars, _activeStar);
            if (selected)
                SetButtonStatusDisplay(button, _selectedButtonDisplay);
            else if (locked)
                SetButtonStatusDisplay(button, _lockedButtonDisplay);
            else
                SetButtonStatusDisplay(button, _unlockedButtonDisplay);
        }

        private void SetButtonStatusDisplay(LevelSelectionButton button, LevelSelectionButtonStatusDisplay display)
        {
            button.SetColor(display.Color);
            if(display.Sprite)
                button.SetSprite(display.Sprite);
        }

        private void UpdateSelectedLevel(int level)
        {
            SetButtonStatusDisplay(_levelSelectionButtons[_lastSelectedButtonID], _unlockedButtonDisplay);
            OnSelectedLevelChanged?.Invoke(level);

            _lastSelectedButtonID = level;
            SetButtonStatusDisplay(_levelSelectionButtons[_lastSelectedButtonID], _selectedButtonDisplay);
        }
    }

    [Serializable]
    public class LevelSelectionButtonStatusDisplay
    {
        [Header("If Sprite is Null, it won't be set")]
        public Sprite Sprite;
        public Color Color = Color.white;
    }
}
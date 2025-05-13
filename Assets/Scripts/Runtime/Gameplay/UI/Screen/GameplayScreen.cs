using System;
using DG.Tweening;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Gameplay.UI.Screen
{
    public class GameplayScreen : UiScreen
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _ballsAmountText;
        
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _activeStar;
        [SerializeField] private float _progressAnimDuration;
        
        public event Action OnSettingsPressed;
        public event Action OnPausePressed;

        private LevelProgressTracker _progressTracker;
        private GameData _gameData;
        
        [Inject]
        private void Construct(LevelProgressTracker levelProgressTracker, GameData gameData)
        {
            _levelText.text = $"Level {gameData.LevelID + 1}";
            
            _progressTracker = levelProgressTracker;
            _progressTracker.OnProgressChanged += UpdateProgress;
            _progressTracker.OnStarEarned += UpdateStarsEarned;
            
            _gameData = gameData;
            _gameData.OnBallLaunched += UpdateBallsText;
        }

        private void UpdateBallsText(int amount)
        {
            _ballsAmountText.text = (_gameData.MaxLaunchedBalls - amount).ToString();
        }

        private void OnDestroy()
        {
            _progressTracker.OnProgressChanged -= UpdateProgress;
            _progressTracker.OnStarEarned -= UpdateStarsEarned;
            _gameData.OnBallLaunched -= UpdateBallsText;
        }

        private void UpdateProgress(float value)
        {
            _progressSlider.DOValue(value, _progressAnimDuration).
                SetEase(Ease.Linear).
                SetLink(gameObject);
        }
        
        private void UpdateStarsEarned(int value)
        {
            _stars[value - 1].sprite = _activeStar;
            _stars[value - 1].transform.DOPunchScale(Vector3.one * 1.3f , _progressAnimDuration).SetLink(gameObject);
        }

        public void Initialize()
        {
            _settingsButton.onClick.AddListener(() => OnSettingsPressed?.Invoke());
            _pauseButton.onClick.AddListener(() => OnPausePressed?.Invoke());
        }
    }
}
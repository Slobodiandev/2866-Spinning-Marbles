using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Controllers;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.Gameplay;

namespace Runtime.Gameplay.Game
{
    public class GameStateMachine : StateMachineController
    {
        private readonly StateMachine _stateMachine;

        private readonly MenuStateMachineController _menuStateMachineController;
        private readonly LevelSelectionStateMachineController _levelSelectionMachineController;
        private readonly ShopStateMachineController _shopStateMachineController;
        private readonly AccountScreenStateMachineController _accountScreenStateMachineController;
        private readonly ApplicationStateChangeController _applicationStateChangeController;
        private readonly InitShopStateMachine _initShopStateMachine;
        private readonly GameplayScreenMachineController _gameplayScreenMachineController;
        private readonly InventoryScreenMachineController _inventoryScreenMachineController;
        private readonly SettingsScreenMachineController _settingsScreenMachineController;
        private readonly DailyRewardScreenMachineController _dailyRewardScreenMachineController;
        private readonly TermsOfUseMachineController _termsOfUseMachineController;
        private readonly PrivacyPolicyMachineController _privacyPolicyMachineController;
        private readonly MainScreenMachineController _mainScreenMachineController;
        private readonly PausePopupStateMachineController _pausePopupStateMachineController;
        private readonly ItemPrizePopupStateMachineController _itemPrizePopupStateMachineController;
        private readonly GameResultMachineController _gameResultMachineController;
        private readonly LosePopupStateMachineController _losePopupStateMachineController;

        public GameStateMachine(ICustomLogger customLogger,
            MenuStateMachineController menuStateMachineController,
            LevelSelectionStateMachineController levelSelectionMachineController,
            ShopStateMachineController shopStateMachineController,
            AccountScreenStateMachineController accountScreenStateMachineController,
            StateMachine stateMachine,
            ApplicationStateChangeController applicationStateChangeController,
            InitShopStateMachine initShopStateMachine,
            GameplayScreenMachineController gameplayScreenMachineController,
            InventoryScreenMachineController inventoryScreenMachineController,
            SettingsScreenMachineController settingsScreenMachineController,
            DailyRewardScreenMachineController dailyRewardScreenMachineController,
            TermsOfUseMachineController termsOfUseMachineController,
            PrivacyPolicyMachineController privacyPolicyMachineController,
            MainScreenMachineController mainScreenMachineController,
            PausePopupStateMachineController pausePopupStateMachineController,
            ItemPrizePopupStateMachineController itemPrizePopupStateMachineController,
            GameResultMachineController gameResultMachineController,
            LosePopupStateMachineController losePopupStateMachineController) : base(customLogger)
        {
            _stateMachine = stateMachine;
            _menuStateMachineController = menuStateMachineController;
            _levelSelectionMachineController = levelSelectionMachineController;
            _shopStateMachineController = shopStateMachineController;
            _accountScreenStateMachineController = accountScreenStateMachineController;
            _applicationStateChangeController = applicationStateChangeController;
            _initShopStateMachine = initShopStateMachine;
            _gameplayScreenMachineController = gameplayScreenMachineController;
            _inventoryScreenMachineController = inventoryScreenMachineController;
            _settingsScreenMachineController = settingsScreenMachineController;
            _dailyRewardScreenMachineController = dailyRewardScreenMachineController;
            _termsOfUseMachineController = termsOfUseMachineController;
            _privacyPolicyMachineController = privacyPolicyMachineController;
            _mainScreenMachineController = mainScreenMachineController;
            _pausePopupStateMachineController = pausePopupStateMachineController;
            _itemPrizePopupStateMachineController = itemPrizePopupStateMachineController;
            _gameResultMachineController = gameResultMachineController;
            _losePopupStateMachineController = losePopupStateMachineController;
        }

        public override async UniTask EnterState(CancellationToken cancellationToken)
        {
            await _applicationStateChangeController.RunController(default);

            _stateMachine.InitializeStateMachine(_menuStateMachineController, _levelSelectionMachineController, _shopStateMachineController, 
                _initShopStateMachine, _accountScreenStateMachineController, _gameplayScreenMachineController,
                _inventoryScreenMachineController, _settingsScreenMachineController, _dailyRewardScreenMachineController, 
                _termsOfUseMachineController, _privacyPolicyMachineController, _mainScreenMachineController, _pausePopupStateMachineController,
                _itemPrizePopupStateMachineController, _gameResultMachineController, _losePopupStateMachineController);
            _stateMachine.GoToState<MainScreenMachineController>().Forget();
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.DailyLogin;
using Runtime.Gameplay.Game.Controllers;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData.Data;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class DailyRewardScreenMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly IUserInventoryService _userInventoryService;
        private readonly DailyLoginService _dailyLoginService;
        private readonly RouletteSpinner _rouletteSpinner;
        private readonly DailyLoginRouletteFactory _dailyLoginFactory;
        private readonly RouletteSpinResultCalculator _rouletteSpinResultCalculator;
        private readonly ItemPrizePopupStateMachineController _itemPrizePopupStateMachineController;

        private DailyRewardScreen _screen;

        public DailyRewardScreenMachineController(ICustomLogger customLogger, IUiService uiService,
            IUserInventoryService userInventoryService, DailyLoginService dailyLoginService,
            RouletteSpinner rouletteSpinner, DailyLoginRouletteFactory dailyLoginRouletteFactory,
            RouletteSpinResultCalculator rouletteSpinResultCalculator,
            ItemPrizePopupStateMachineController itemPrizePopupStateMachineController) : base(customLogger)
        {
            _uiService = uiService;
            _userInventoryService = userInventoryService;
            _dailyLoginService = dailyLoginService;
            _rouletteSpinner = rouletteSpinner;
            _dailyLoginFactory = dailyLoginRouletteFactory;
            _rouletteSpinResultCalculator = rouletteSpinResultCalculator;
            _itemPrizePopupStateMachineController = itemPrizePopupStateMachineController;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.DailyRewardScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<DailyRewardScreen>(ConstScreens.DailyRewardScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();

            for (var i = 0; i < _screen.Columns.Length; i++)
                _screen.ParentRouletteItems(_dailyLoginFactory.CreateRouletteItemViews(), i);

            if (!_dailyLoginService.SpinAvailable())
                _screen.DisableRoulette();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoToState<MainScreenMachineController>();
            _screen.OnHelpPressed += async () => await _uiService.ShowWindow(ConstPopups.InfoPopup);
            _screen.OnSpinPressed += ProcessSpinRoulette;
        }

        private async void ProcessSpinRoulette()
        {
            _screen.EnableFlowComponents(false);

            var columns = _screen.Columns;

            var targetIndexes = _rouletteSpinResultCalculator.GetTargetItemIndexes(columns);

            var spinTasks = CreateSpinTasks(targetIndexes);

            await UniTask.WhenAll(spinTasks);

            _screen.EnableFlowComponents(true);

            ProcessWinnings();
        }

        private UniTask[] CreateSpinTasks(List<int> targetIndexes)
        {
            var columns = _screen.Columns;
            var spinTasks = new UniTask[columns.Length];

            var childCount = columns[0].transform.childCount;
            var middleElementIndex = childCount / 2;

            int additionalSpins = childCount * columns.Length;
            
            for (var i = 0; i < columns.Length; i++)
            {
                var steps = CalculateSpinSteps(targetIndexes[i], middleElementIndex, childCount, additionalSpins);
                additionalSpins -= childCount;
                
                spinTasks[i] = _rouletteSpinner.Spin(columns[i], steps);
            }

            return spinTasks;
        }

        private int CalculateSpinSteps(int targetIndex, int middleElementIndex, int childCount, int additionalSpins)
        {
            var stepsToAlign = (middleElementIndex - targetIndex + childCount) % childCount;
            return stepsToAlign + additionalSpins;
        }

        private void ProcessWinnings()
        {
            int reward = _rouletteSpinResultCalculator.CalculateCoinReward();
            _itemPrizePopupStateMachineController.SetReward(reward);
            _itemPrizePopupStateMachineController.EnterState().Forget();
            _userInventoryService.AddBalance(reward);
            _dailyLoginService.RecordSpinDate();
        }
    }
}
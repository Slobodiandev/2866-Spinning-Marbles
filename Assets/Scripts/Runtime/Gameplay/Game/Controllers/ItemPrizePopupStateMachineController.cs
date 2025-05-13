using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Popup;

namespace Runtime.Gameplay.Game.Controllers
{
    public class ItemPrizePopupStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;

        private int _reward;
    
        public ItemPrizePopupStateMachineController(ICustomLogger customLogger, IUiService uiService) : base(customLogger)
        {
            _uiService = uiService;
        }

        public override async UniTask EnterState(CancellationToken cancellationToken = default)
        {
            ItemPrizeWindow window = await _uiService.ShowWindow(ConstPopups.ItemPrizePopup) as ItemPrizeWindow;

            window.SetReward(_reward);
        
            window.OnClaimButtonPressed += async () =>
            {
                window.DestroyWindow();
                await GoToState<MainScreenMachineController>();
            };
        }
    
        public void SetReward(int reward) => _reward = reward;
    }
}

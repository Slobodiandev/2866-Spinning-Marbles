using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.Services.Shop;
using Runtime.Gameplay.Services.UI;

namespace Runtime.Gameplay.Game.Controllers
{
    public class ShopStateMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly IShopItemsStorage _shopItemsStorage;
        private readonly IProcessPurchaseService _processPurchaseService;

        private CancellationTokenSource _cancellationTokenSource;

        public ShopStateMachineController(ICustomLogger customLogger, IUiService uiService, IShopItemsStorage shopItemsStorage, 
                IProcessPurchaseService processPurchaseService) : base(customLogger)
        {
            _uiService = uiService;
            _shopItemsStorage = shopItemsStorage;
            _processPurchaseService = processPurchaseService;
        }

        public override UniTask EnterState(CancellationToken cancellationToken = default)
        {
            _cancellationTokenSource = new();
            
            Subscribe(cancellationToken);
            
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            _shopItemsStorage.Cleanup();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            
            await _uiService.HideScreen(ConstScreens.ShopScreen);
        }

        private void Subscribe(CancellationToken cancellationToken)
        {
            foreach (var shopItemDisplay in _shopItemsStorage.GetItemDisplay())
                shopItemDisplay.OnPurchasePressed += _ =>  _processPurchaseService.ProcessPurchaseAttempt(shopItemDisplay, cancellationToken);
        }
    }
}
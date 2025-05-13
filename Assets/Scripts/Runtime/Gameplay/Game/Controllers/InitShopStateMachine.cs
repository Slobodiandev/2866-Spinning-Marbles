using System.Collections.Generic;
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.Services.Shop;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Controllers
{
    public class InitShopStateMachine : StateMachineController
    {
        private readonly IShopItemsStorage _shopItemsStorage;
        private readonly IUiService _uiService;
        private readonly IShopItemsDisplayService _shopItemsDisplayService;
        private readonly IConfigProvider _configProvider;
        private readonly ShopService _shopService;
        private readonly ShopItemDisplayController _shopItemDisplayController;
        private readonly List<ISetShopSetup> _setShopConfigs;

        public InitShopStateMachine(ICustomLogger customLogger, IShopItemsStorage shopItemsStorage, IUiService uiService, 
                IShopItemsDisplayService shopItemsDisplayService, IConfigProvider configProvider,
                IProcessPurchaseService processPurchaseService, IPurchaseEffectsService purchaseEffectsService,
                ISelectPurchaseItemService selectPurchaseItemService, ShopService shopService, ShopItemDisplayController shopItemDisplayController) : base(customLogger)
        {
            _shopItemsStorage = shopItemsStorage;
            _uiService = uiService;
            _shopItemsDisplayService = shopItemsDisplayService;
            _configProvider = configProvider;
            _shopService = shopService;
            _shopItemDisplayController = shopItemDisplayController;

            _setShopConfigs = new()
            {
                processPurchaseService,
                purchaseEffectsService,
                selectPurchaseItemService,
                shopItemsDisplayService
            };
        }

        public override UniTask EnterState(CancellationToken cancellationToken = default)
        {
            SetShopConfig();
            var screen = CreateScreen();
            Subscribe(screen);
            
            return GoToState<ShopStateMachineController>(cancellationToken);
        }

        private ShopScreen CreateScreen()
        {
            var screen = _uiService.GetScreen<ShopScreen>(ConstScreens.ShopScreen);
            screen.ShowAsync().Forget();
            Initialize(screen);

            return screen;
        }

        private void Initialize(ShopScreen screen)
        {
            _shopItemDisplayController.SetShop(_shopService);
            _shopItemsDisplayService.CreateShopItems();
            _shopItemsDisplayService.UpdateItemsStatus();
            screen.SetShopItems(_shopItemsStorage.GetItemDisplay());
        }

        private void Subscribe(ShopScreen screen) =>
                screen.OnBackPressed += () => GoToState<MainScreenMachineController>().Forget(); 
        
        private void SetShopConfig()
        {
            var shopConfig = _configProvider.Get<ShopSetup>();

            _shopItemsStorage.SetShopStateConfigs(shopConfig.ShopItemStateConfigs);

            foreach (var setShopConfig in _setShopConfigs)
                setShopConfig.SetShopSetup(shopConfig);
        }
    }
}
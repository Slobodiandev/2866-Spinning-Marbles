using System.Collections.Generic;
using System.Threading;
using Core;
using Core.StateMachine;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.Inventory;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.UI.Screen;

namespace Runtime.Gameplay.Game.Menu
{
    public class InventoryScreenMachineController : StateMachineController
    {
        private readonly IUiService _uiService;
        private readonly SelectedItemsFactory _selectedItemsFactory;
        private readonly ItemSelectionController _itemSelectionController;

        private InventoryScreen _screen;
        
        private List<InventoryItemDisplay> _inventoryItemDisplayList;

        public InventoryScreenMachineController(ICustomLogger customLogger, IUiService uiService, 
            SelectedItemsFactory selectedItemsFactory, ItemSelectionController itemSelectionController) : base(customLogger)
        {
            _uiService = uiService;
            _selectedItemsFactory = selectedItemsFactory;
            _itemSelectionController = itemSelectionController;
        }

        public override UniTask EnterState(CancellationToken cancellationToken)
        {
            CreateScreen();
            SubscribeToEvents();
            CreateInventoryItems();
            
            return UniTask.CompletedTask;
        }

        public override async UniTask ExitState()
        {
            await _uiService.HideScreen(ConstScreens.InventoryScreen);
        }

        private void CreateScreen()
        {
            _screen = _uiService.GetScreen<InventoryScreen>(ConstScreens.InventoryScreen);
            _screen.Initialize();
            _screen.ShowAsync().Forget();
        }

        private void SubscribeToEvents()
        {
            _screen.OnBackPressed += async () => await GoToState<MenuStateMachineController>();
        }

        private void CreateInventoryItems()
        {
            _inventoryItemDisplayList = _selectedItemsFactory.CreateInventoryItemDisplayList();
            _screen.ParentItems(_inventoryItemDisplayList);

            foreach (var item in _inventoryItemDisplayList)
            {
                item.OnSelectPressed += ProcessNewItemSelectionAttempt;
            }
        }

        private void ProcessNewItemSelectionAttempt(SelectedItemModel model)
        {
            _itemSelectionController.SetItemModel(model);
            _itemSelectionController.RunController(CancellationToken.None).Forget();
        }
    }
}
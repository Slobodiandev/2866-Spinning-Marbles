using Runtime.Gameplay.DailyLogin;
using Runtime.Gameplay.Game.Controllers;
using Runtime.Gameplay.Game.Menu;
using Runtime.Gameplay.Gameplay;
using Runtime.Gameplay.Gameplay.Misc;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.Gameplay.Systems;
using Runtime.Gameplay.Gameplay.Visuals;
using Runtime.Gameplay.Inventory;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.Services.Shop;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Game
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            BindStateControllers();
            BindController();
            BindServices();
            BindRoulette();
            BindData();
            BindSystems();
        }

        private void BindData()
        {
            Container.Bind<IShopItemsStorage>().To<ShopItemsStorage>().AsSingle();
            Container.Bind<ShopItemDisplayModel>().AsTransient();
            Container.Bind<GameData>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IProcessPurchaseService>().To<ProcessPurchaseService>().AsSingle();
            Container.Bind<ISelectPurchaseItemService>().To<SelectPurchaseItemService>().AsSingle();
            Container.Bind<IPurchaseEffectsService>().To<PurchaseEffectsService>().AsSingle();
            Container.Bind<IShopItemsDisplayService>().To<ShopItemsDisplayService>().AsSingle();
        }

        private void BindController()
        {
            Container.Bind<SettingsPopupStateController>().AsSingle();
            Container.Bind<UserProgressService>().AsSingle();
            Container.Bind<ShopItemDisplayController>().AsSingle();
            Container.Bind<ItemSelectionController>().AsSingle();
            Container.Bind<LevelProgressTracker>().AsSingle();
            Container.Bind<BallsDestructionController>().AsSingle().NonLazy();
            Container.Bind<SkinSetter>().AsSingle().NonLazy();
            Container.Bind<CoinsRewardCalculator>().AsSingle().NonLazy();
            Container.Bind<LevelIntitializer>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SelectedItemsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryItemSelectionFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<LaunchedBallsFactory>().AsSingle();
        }

        private void BindSystems()
        {
            Container.Bind<GameplaySystemsManager>().AsSingle();
            Container.Bind<MenuSystemsManager>().AsSingle();
            Container.Bind<GameSetupController>().AsSingle();
            Container.Bind<BallCollisionProcessor>().AsSingle().NonLazy();
        }
        
        private void BindStateControllers()
        {
            Container.Bind<InitShopStateMachine>().AsSingle();
            Container.Bind<InventoryScreenMachineController>().AsSingle();
            Container.Bind<ShopStateMachineController>().AsSingle();
            Container.Bind<SettingsScreenMachineController>().AsSingle();
            Container.Bind<PausePopupStateMachineController>().AsSingle();
            Container.Bind<TermsOfUseMachineController>().AsSingle();
            Container.Bind<PrivacyPolicyMachineController>().AsSingle();
            Container.Bind<DailyRewardScreenMachineController>().AsSingle();
            Container.Bind<MainScreenMachineController>().AsSingle();
            Container.Bind<GameplayScreenMachineController>().AsSingle();
            Container.Bind<LevelSelectionStateMachineController>().AsSingle();
            Container.Bind<AccountScreenStateMachineController>().AsSingle();
            Container.Bind<MenuStateMachineController>().AsSingle();
            Container.Bind<GameResultMachineController>().AsSingle();
            Container.Bind<LosePopupStateMachineController>().AsSingle();
        }
        
        private void BindRoulette()
        {
            Container.BindInterfacesAndSelfTo<DailyLoginRouletteFactory>().AsSingle();
            Container.Bind<RouletteSpinner>().AsSingle();
            Container.Bind<RouletteSpinResultCalculator>().AsSingle();
            Container.Bind<ItemPrizePopupStateMachineController>().AsSingle();
        }
    }
}
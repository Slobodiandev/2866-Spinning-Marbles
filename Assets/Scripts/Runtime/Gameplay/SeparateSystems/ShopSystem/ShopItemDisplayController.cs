namespace Runtime.Gameplay.SeparateSystems.ShopSystem
{
    public class ShopItemDisplayController 
    {
        private readonly ShopItemDisplayModel _shopItemDisplayModel;
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly IShopItemsStorage _shopItemsStorage;
        
        private ShopService _shopService;

        public ShopItemDisplayController(GameObjectFactory gameObjectFactory, IShopItemsStorage shopItemsStorage)
        {
            _gameObjectFactory = gameObjectFactory;
            _shopItemsStorage = shopItemsStorage;
        }

        public void SetShop(ShopService shopService) =>
                _shopService = shopService;

        public void CreateItemDisplayView(ShopItem shopItem)
        {
            var shopItemDisplay = _gameObjectFactory.Create(shopItem.ShopItemDisplayView);
            _shopItemsStorage.AddItemDisplay(shopItemDisplay);
            shopItemDisplay.SetShopItem(shopItem);
            SetItemState(shopItemDisplay.GetShopItemModel());
        }

        public void SortItemsByNotPurchased() => _shopItemsStorage.SortItemsByNotPurchased();

        public void UpdateItemStates()
        {
            foreach (var itemDisplayView in _shopItemsStorage.GetItemDisplay())
            {
                var shopItemDisplayModel = itemDisplayView.GetShopItemModel();
                itemDisplayView.UpdateShopItemUI(_shopItemsStorage.GetItemStateConfig(shopItemDisplayModel.ItemState));
            }
        }

        private void SetItemState(ShopItemDisplayModel shopItemDisplayModel)
        {
            shopItemDisplayModel.SetShopItemState(_shopService.IsItemPurchased(shopItemDisplayModel.ShopItem) ? ShopItemState.Purchased : ShopItemState.NotPurchased);
        }
    }
}
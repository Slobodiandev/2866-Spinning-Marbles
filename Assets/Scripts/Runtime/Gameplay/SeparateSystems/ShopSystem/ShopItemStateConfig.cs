using Core;
using UnityEngine;

namespace Runtime.Gameplay.SeparateSystems.ShopSystem
{
    [CreateAssetMenu(fileName = "ShopItemsConfig", menuName = "Config/ShopItemsConfig")]
    public class ShopItemStateConfig : BaseConfig
    {
        [SerializeField] private ShopItemState _shopItemState;
        public ShopItemState ShopItemState => _shopItemState;
    }
}
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Runtime.Gameplay.DailyLogin
{
    [CreateAssetMenu(fileName = "RouletteItemsConfig", menuName = "Config/RouletteItemsConfig")]
    public class RouletteItemsConfig : BaseConfig
    {
        public List<RouletteItemModel> RouletteItemModels;
    }
}

using System.Collections.Generic;
using Core;
using Runtime.Gameplay.Services.SettingsProvider;
using Runtime.Gameplay.Tools;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.DailyLogin
{
    public class DailyLoginRouletteFactory : IInitializable
    {
        private readonly GameObjectFactory _factory;
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigProvider _configProvider;
    
        private GameObject _rouletteItemPrefab;

        public DailyLoginRouletteFactory(GameObjectFactory factory, IAssetProvider assetProvider,
            IConfigProvider configProvider)
        {
            _factory = factory;
            _assetProvider = assetProvider;
            _configProvider = configProvider;
        }
    
        public async void Initialize()
        {
            _rouletteItemPrefab = await _assetProvider.LoadAsset<GameObject>(ConstPrefabs.RouletteItemPrefab);
        }

        public List<RouletteItemView> CreateRouletteItemViews()
        {
            List<RouletteItemView> result = new ();

            var itemsConfig = _configProvider.Get<RouletteItemsConfig>();

            for (int i = 0; i < itemsConfig.RouletteItemModels.Count; i++)
            {
                var itemModel = itemsConfig.RouletteItemModels[i];

                for (int j = 0; j < itemModel.SpawnAmount; j++)
                {
                    var newItem = _factory.Create<RouletteItemView>(_rouletteItemPrefab);
                    newItem.Initialize(itemModel);
                    result.Add(newItem);
                }
            }
        
            Helper.Shuffle(result);
        
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using Core;
using Core.Services.Audio;
using Cysharp.Threading.Tasks;
using Runtime.Gameplay.DailyLogin;
using Runtime.Gameplay.Gameplay.Configs;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.SeparateSystems.UserAccount;
using Runtime.Gameplay.Services.ScreenOrientation;

namespace Runtime.Gameplay.Services.SettingsProvider
{
    public class ConfigsProvider : IConfigProvider
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<Type, BaseConfig> _settings = new Dictionary<Type, BaseConfig>();

        public ConfigsProvider(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask Initialize()
        {
            var screenOrientationConfig = await _assetProvider.LoadAsset<ScreenOrientationConfig>(ConstConfigs.ScreenOrientationConfig);
            var audioConfig = await _assetProvider.LoadAsset<AudioConfig>(ConstConfigs.AudioConfig);
            var gameConfig = await _assetProvider.LoadAsset<GameConfig>(ConstConfigs.GameConfig);          
            var shopSetup = await _assetProvider.LoadAsset<ShopSetup>(ConstConfigs.ShopSetup);
            var validationConfig = await _assetProvider.LoadAsset<UserDataValidationConfig>(ConstConfigs.UserDataValidationConfig);
            var avatarsConfig = await _assetProvider.LoadAsset<DefaultAvatarsConfig>(ConstConfigs.DefaultAvatarsConfig);
            var rouletteItemsConfig = await _assetProvider.LoadAsset<RouletteItemsConfig>(ConstConfigs.RouletteItemsConfig);
            var collisionProcessorConfig = await _assetProvider.LoadAsset<CollisionProcessorConfig>(ConstConfigs.CollisionProcessorConfig);
            var levelClearConfig = await _assetProvider.LoadAsset<LevelClearConditionConfig>(ConstConfigs.LevelClearConditionConfig);
            
            Set(screenOrientationConfig);
            Set(audioConfig);
            Set(validationConfig);
            Set(avatarsConfig);
            Set(gameConfig);
            Set(shopSetup);
            Set(rouletteItemsConfig);
            Set(collisionProcessorConfig);
            Set(levelClearConfig);
        }

        public T Get<T>() where T : BaseConfig
        {
            if (_settings.ContainsKey(typeof(T)))
            {
                var setting = _settings[typeof(T)];
                return setting as T;
            }

            throw new Exception("No setting found");
        }

        public void Set(BaseConfig config)
        {
            if (_settings.ContainsKey(config.GetType()))
                return;

            _settings.Add(config.GetType(), config);
        }
    }
}
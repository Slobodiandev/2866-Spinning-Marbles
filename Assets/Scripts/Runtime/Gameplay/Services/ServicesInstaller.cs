using Core;
using Core.Compressor;
using Core.Services.Audio;
using Runtime.Gameplay.DailyLogin;
using Runtime.Gameplay.SeparateSystems.ShopSystem;
using Runtime.Gameplay.SeparateSystems.UserAccount;
using Runtime.Gameplay.Services.ApplicationState;
using Runtime.Gameplay.Services.Audio;
using Runtime.Gameplay.Services.IAP;
using Runtime.Gameplay.Services.NetworkConnection;
using Runtime.Gameplay.Services.ScreenOrientation;
using Runtime.Gameplay.Services.SettingsProvider;
using Runtime.Gameplay.Services.UI;
using Runtime.Gameplay.Services.UserData;
using Runtime.Gameplay.Services.UserData.Data;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Services
{
    [CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/ServicesInstaller")]
    public class ServicesInstaller : ScriptableObjectInstaller<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            BindUiServices();
            BindPersistenceServices();
            BindAudioServices();
            BindNetworkingServices();
            BindApplicationServices();
            BindScreenOrientationServices();
            BindShopServices();
        }

        private void BindUiServices()
        {
            Container.Bind<IUiService>().To<UiService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IUserInventoryService>().To<UserInventoryService>().AsSingle();
            Container.Bind<AvatarSelectionService>().AsSingle();
        }

        private void BindPersistenceServices()
        {
            Container.Bind<IPersistentDataProvider>().To<PersistantDataProvider>().AsSingle();
            Container.Bind<IFileStorageService>().To<PersistentFileStorageService>().AsSingle();
            Container.Bind<IConfigProvider>().To<ConfigsProvider>().AsSingle();
            Container.Bind<ISerializationProvider>().To<JsonSerializationProvider>().AsSingle();
            Container.Bind<UserDataService>().AsSingle();
            Container.Bind<UserDataValidationService>().AsSingle();
        }

        private void BindAudioServices()
        {
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            Container.Bind<IIAPService>().To<IAPServiceMock>().AsSingle();
        }

        private void BindNetworkingServices()
        {
            Container.Bind<INetworkConnectionService>().To<NetworkConnectionService>().AsSingle();
            Container.Bind<WebRequestController>().AsSingle();
        }

        private void BindApplicationServices()
        {
            Container.Bind<ICustomLogger>().To<CustomLogger>().AsSingle();
            Container.Bind<BaseCompressor>().To<ZipCompressor>().AsSingle();
            Container.Bind<GameObjectFactory>().AsSingle();
            Container.Bind<ApplicationStateService>().AsSingle();
            Container.Bind<UserAccountService>().AsSingle();
            Container.Bind<ImageProcessingService>().AsSingle();
            Container.Bind<DailyLoginService>().AsSingle();
        }

        private void BindScreenOrientationServices()
        {
            Container.BindInterfacesAndSelfTo<ScreenOrientationAlertController>().AsSingle();
        }

        private void BindShopServices()
        {
            Container.BindInterfacesAndSelfTo<ShopService>().AsSingle();
        }
    }
}
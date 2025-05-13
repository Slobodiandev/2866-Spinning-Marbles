using Core;
using Core.Compressor;
using Runtime.Gameplay.Services.Path;

namespace Runtime.Gameplay.Services.UserData
{
    public class UserDataService
    {
        private readonly IPersistentDataProvider _persistentDataProvider;
        private readonly BaseCompressor _compressor;

        private Data.UserData _userData;

        public UserDataService(IPersistentDataProvider persistentDataProvider, BaseCompressor compressor)
        {
            _persistentDataProvider = persistentDataProvider;
            _compressor = compressor;
        }

        public void Initialize()
        {
#if DEV
            _userData = _persistentDataProvider.LoadData<Data.UserData>(ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName) ?? new Data.UserData();
#else
            _userData = _persistentDataProvider.LoadData<Data.UserData>(ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName,null, _compressor) ?? new Data.UserData();
#endif
        }

        public Data.UserData RetrieveUserData() => _userData;

        public void SaveUserData()
        {
            if(_userData == null)
                return;

#if DEV
            _persistentDataProvider.SaveData(_userData, ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName);
#else
            _persistentDataProvider.SaveData(_userData, ConstDataPath.UserDataPath, ConstDataPath.UserDataFileName, null, _compressor);
#endif
        }
    }
}
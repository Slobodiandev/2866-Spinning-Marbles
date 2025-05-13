using Core.Compressor;

namespace Core
{
    public interface IPersistentDataProvider
    {
        T LoadData<T>(string path, string fileName, ISerializationProvider serializationProvider = null, BaseCompressor compressor = null) where T : class;

        bool SaveData<T>(T data, string path, string fileName, ISerializationProvider serializationProvider = null, BaseCompressor compressor = null) where T : class;
    }
}
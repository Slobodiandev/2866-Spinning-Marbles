using System;
using Core.Compressor;

namespace Core
{
    public class PersistantDataProvider : IPersistentDataProvider
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ISerializationProvider _defaultSerializationProvider;

        public PersistantDataProvider(IFileStorageService fileStorageService,
            ISerializationProvider defaultSerializationProvider)
        {
            _fileStorageService = fileStorageService;
            _defaultSerializationProvider = defaultSerializationProvider;
        }

        public T LoadData<T>(string path, string fileName, ISerializationProvider serializationProvider = null, BaseCompressor compressor = null) where T : class
        {
            try
            {
                var text = _fileStorageService.LoadTextData(path, fileName);

                if (string.IsNullOrEmpty(text))
                    return null;

                if (compressor != null)
                    text = compressor.DecompressData(text);

                var serializer = serializationProvider ?? _defaultSerializationProvider;
                return string.IsNullOrEmpty(text) ? default : serializer.Deserialize<T>(text);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool SaveData<T>(T data, string path, string fileName, ISerializationProvider serializationProvider = null, BaseCompressor compressor = null) where T : class
        {
            try
            {
                var serializer = serializationProvider ?? _defaultSerializationProvider;

                var text = serializer.Serialize(data);
                if (compressor != null)
                    text = compressor.CompressData(text);
                _fileStorageService.SaveTextData(text, path, fileName);
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
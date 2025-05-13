using System;
using System.IO;

namespace Core
{
    public sealed class PersistentFileStorageService : IFileStorageService
    {
        private readonly ICustomLogger _customLogger;

        public PersistentFileStorageService(ICustomLogger customLogger)
        {
            _customLogger = customLogger;
        }

        public void SaveTextData(string data, string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                _customLogger.Error("Failed save text, no file name");
                return;
            }
            
            if(string.IsNullOrEmpty(filePath))
            {
                _customLogger.Error("Failed save text, no file path");
                return;
            }

            if(string.IsNullOrEmpty(data))
            {
                _customLogger.Error("Failed save text, data is empty");
                return;
            }

            try
            {
                if(!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                var path = Path.Combine(filePath, fileName);
                File.WriteAllText(path, data);
            }
            catch (Exception e)
            {
                _customLogger.Error($"Failed save text: {e}");
            }
        }

        public string LoadTextData(string filePath, string fileName)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                _customLogger.Error("Failed load text, path is empty");
                return null;
            }

            if(string.IsNullOrEmpty(fileName))
            {
                _customLogger.Error("Failed load text, no file name");
                return null;
            }

            if(!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                _customLogger.Warning($"Directory not exist: {filePath}");
                return null;
            }

            var path = Path.Combine(filePath, fileName);

            if(!File.Exists(path))
            {
                _customLogger.Warning($"File not exist: {path}");
                return null;
            }

            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                _customLogger.Error($"Failed load text: {e}");
                return null;
            }
        }
    }
}
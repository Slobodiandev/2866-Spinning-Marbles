namespace Core
{
    public interface IFileStorageService
    {
        void SaveTextData(string data, string filePath, string fileName);
        string LoadTextData(string filePath, string fileName);
    }
}
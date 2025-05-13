namespace Core.Compressor
{
    public abstract class BaseCompressor
    {
        public abstract string CompressData(string data);
        public abstract string DecompressData(string data);
    }
}
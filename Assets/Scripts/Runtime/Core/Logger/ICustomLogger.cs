namespace Core
{
    public interface ICustomLogger
    {
        void Log(object message);
        void Warning(object message);
        void Error(object message);
    }
}
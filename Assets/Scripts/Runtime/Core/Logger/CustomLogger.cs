namespace Core
{
    public class CustomLogger : ICustomLogger
    {
        public void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        public void Warning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        public void Error(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
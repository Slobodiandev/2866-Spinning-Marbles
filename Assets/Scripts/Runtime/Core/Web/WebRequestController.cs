using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Core
{
    public class WebRequestController
    {
        private readonly ICustomLogger _customLogger;

        public WebRequestController(ICustomLogger customLogger)
        {
            _customLogger = customLogger;
        }

        public async UniTask<string> SendGetRequestAsync(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                var operation = await webRequest.SendWebRequest().ToUniTask();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    _customLogger.Error("Error: " + webRequest.error);
                    return null;
                }

                _customLogger.Log("Received: " + webRequest.downloadHandler.text);
                return webRequest.downloadHandler.text;
            }
        }

        public async UniTask<string> SendPostRequestAsync(string jsonData, string url)
        {
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

            using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
            {
                webRequest.uploadHandler = new UploadHandlerRaw(postData);
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Content-Type", "application/json");

                var operation = await webRequest.SendWebRequest().ToUniTask();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    _customLogger.Error("Error: " + webRequest.error);
                    return null;
                }

                _customLogger.Log("Received: " + webRequest.downloadHandler.text);
                return webRequest.downloadHandler.text;
            }
        }
    }
}
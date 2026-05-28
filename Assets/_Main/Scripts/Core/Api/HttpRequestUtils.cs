using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequestUtils
{
    public static IEnumerator GetRequest<T>(string url, Action<T> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Successful Get Request");
                    T data = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    callback.Invoke(data);
                    break;
            }
        }
    }

    public static IEnumerator PostRequest<T>(string url, object body, Action<T> callback)
    {
        string json = JsonUtility.ToJson(body);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("Error: " + webRequest.error);
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("HTTP Error: " + webRequest.error);
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("Successful Post Request");

                    T data = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                    callback?.Invoke(data);
                    break;
            }
        }
    }
}
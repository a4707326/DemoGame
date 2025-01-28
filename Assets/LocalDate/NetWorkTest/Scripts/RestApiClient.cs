using UnityEngine;
using UnityEngine.Networking;
using Google.Protobuf; // 使用 Protobuf
using DemoGameService;
using System.Collections;

public class RestApiClient : MonoBehaviour
{
    private const string ApiUrl = "http://127.0.0.1:8888/api/hello";

    public void SendRestRequest(string name)
    {
        var request = new HelloRequest { Name = name };
        byte[] requestData = request.ToByteArray();

        StartCoroutine(PostRequest(ApiUrl, requestData));
    }

    private IEnumerator PostRequest(string url, byte[] requestData)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(requestData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/x-protobuf");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var response = HelloResponse.Parser.ParseFrom(request.downloadHandler.data);
            Debug.Log($"REST API Response: {response.Message}");
        }
        else
        {
            Debug.LogError($"REST API Error: {request.error}");
        }
    }
}

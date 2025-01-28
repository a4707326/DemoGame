using UnityEngine;
using UnityEngine.Networking;
using Google.Protobuf; // 使用 Protobuf
using DemoGameService;
using System.Collections;

public class NetTest : MonoBehaviour
{
    [SerializeField]
    RestApiClient restClient;
    [SerializeField]
    WebSocketClient wsClient;

    private void Start()
    {
        TestRest();
        //TestWebSocket();
        InvokeRepeating("TestWebSocket", 3,3);
    }


    private void TestRest()
    {
        restClient.SendRestRequest("Unity User");
    }
    private void TestWebSocket()
    {
        wsClient.SendWebSocketMessage("Unity User", "Hello WebSocket!");
    }
}

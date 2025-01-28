using UnityEngine;
using NativeWebSocket; // 使用 NativeWebSocket
using Google.Protobuf; // 使用 Protobuf
using DemoGameService;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;
    private const string WebSocketUrl = "ws://127.0.0.1:8888/ws";

    async void Start()
    {
        // 初始化 WebSocket
        ws = new WebSocket(WebSocketUrl);

        // 設置 WebSocket 事件處理
        ws.OnOpen += () =>
        {
            Debug.Log("WebSocket connection opened!");
        };

        ws.OnError += (e) =>
        {
            Debug.LogError($"WebSocket Error: {e}");
        };

        ws.OnClose += (e) =>
        {
            Debug.Log($"WebSocket connection closed: {e}");
        };


        

        ws.OnMessage += (bytes) =>
        {
            Debug.Log("OnMessage received bytes.");

            try
            {
                // 嘗試解析 Protobuf 消息
                var chatMessage = ChatMessage.Parser.ParseFrom(bytes);
                Debug.Log($"WebSocket Received from {chatMessage.Sender}: {chatMessage.Content}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to parse WebSocket message: {ex.Message}");
                Debug.LogError($"Received raw bytes: {System.Text.Encoding.UTF8.GetString(bytes)}");
            }
        };

        // 建立 WebSocket 連接
        await ws.Connect();

        if (ws.State == WebSocketState.Open)
        {
            Debug.Log("WebSocket successfully connected.");
        }
        else
        {
            Debug.LogError("WebSocket failed to connect.");
        }
    }

    void Update()
    {
        // 確保 WebSocket 事件被處理
        if (ws != null)
        {
            ws.DispatchMessageQueue();
        }
    }


    public async void SendWebSocketMessage(string sender, string content)
    {
        if (ws.State == WebSocketState.Open)
        {
            var chatMessage = new ChatMessage
            {
                Sender = sender,
                Content = content
            };

            await ws.Send(chatMessage.ToByteArray());
            Debug.Log($"Sent WebSocket message: {chatMessage.Content}");
        }
        else
        {
            Debug.LogWarning("WebSocket is not connected.");
        }
    }

    async void OnApplicationQuit()
    {
        if (ws != null)
        {
            await ws.Close();
        }
    }
}

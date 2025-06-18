using System;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NaughtyAttributes;
using Cysharp.Threading.Tasks;

namespace Chess_Client.GameScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class NetworkManager : Singleton<NetworkManager>
	{
		private readonly int BUFFER_SIZE = 4096;
		
		private readonly string SERVER_IP = "127.0.0.1";
		private readonly int SERVER_PORT = 14653;
		
		[Header("Config")]
		[SerializeField]
		private int maxReconnectAttempts = 5;
		
		[Header("Status")]
		[SerializeField, ReadOnly]
		private bool connected = false;
		public bool Connected => connected;
		
		[SerializeField, ReadOnly]
		private int currentReconnectAttempts = 0;
		
		private readonly Dictionary<string, Action<object>> eventHandlers = new Dictionary<string, Action<object>>();
		
		private TcpClient client;
		private NetworkStream stream;
		private CancellationTokenSource cts;

		private void Start()
		{
			TryReconnect();
		}

	    public void TryReconnect()
	    {
		    TryReconnectTask().Forget();
	    }

	    private async UniTask TryReconnectTask()
	    {
		    if (connected) return;
		    
	        while (currentReconnectAttempts < maxReconnectAttempts)
	        {
		        connected = await ConnectTask(SERVER_IP, SERVER_PORT);
	            if (connected)
	            {
	                currentReconnectAttempts = 0;
	                return;
	            }

	            currentReconnectAttempts++;
	            Debug.LogWarning($"[NetworkManager] Reconnection attempt {currentReconnectAttempts}/{maxReconnectAttempts}...");
	            await UniTask.Delay(TimeSpan.FromSeconds(2));
	        }
	        
	        Debug.LogError("[NetworkManager] Failed to connect to server.");
	    }

	    private async UniTask<bool> ConnectTask(string host, int port)
	    {
	        try
	        {
	            client = new TcpClient();
	            await client.ConnectAsync(host, port);
	            stream = client.GetStream();
	            cts = new CancellationTokenSource();

	            Debug.Log($"[NetworkManager] Connected to server with {host}:{port}");

	            ReceiveLoopTask(cts.Token).Forget();
	            return true;
	        }
	        catch (Exception e)
	        {
	            Debug.LogError($"[NetworkManager] Connection failed: {e.Message}");
	            return false;
	        }
	    }

	    private async UniTaskVoid ReceiveLoopTask(CancellationToken token)
	    {
	        byte[] buffer = new byte[BUFFER_SIZE];

	        while (!token.IsCancellationRequested)
	        {
	            try
	            {
	                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, token);
	                if (bytesRead == 0)
	                {
	                    Debug.LogWarning("[NetworkManager] Server connection closed (EOF)");
	                    break;
	                }

	                string json = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
	                try
	                {
	                    JObject message = JObject.Parse(json);
	                    string eventName = message["command"]?.ToString().ToLower();

	                    if (eventName != null && eventHandlers.TryGetValue(eventName, out Action<object> callback))
	                    {
	                        UniTask.Post(() => callback(message));
	                    }
	                    else
	                    {
	                        Debug.LogWarning($"[NetworkManager] Unregistered event received: {eventName}");
	                    }
	                }
	                catch (Exception ex)
	                {
	                    Debug.LogError($"[NetworkManager] Failed to parse JSON: {ex.Message}\nRaw: {json}");
	                }
	            }
	            catch (Exception e)
	            {
	                Debug.LogError($"[NetworkManager] Error while receiving data: {e.Message}");
	                break;
	            }
	        }

	        await TryReconnectTask();
	    }
		
	    public void Send(object payload)
	    {
		    SendTask(payload).Forget();
	    }

	    private async UniTask SendTask(object payload)
	    {
		    if (stream == null || !stream.CanWrite)
		    {
			    Debug.LogWarning("[NetworkManager] Failed to send: not connected to server");
			    return;
		    }

		    string json = JsonConvert.SerializeObject(payload);
		    byte[] data = Encoding.UTF8.GetBytes(json);

		    try
		    {
			    await stream.WriteAsync(data, 0, data.Length);
		    }
		    catch (Exception e)
		    {
			    Debug.LogError($"[NetworkManager] Error while sending data: {e.Message}");
		    }
	    }
		
	    public void Disconnect()
	    {
		    if (!connected) return;
			
		    stream?.Close();
		    client?.Close();
			
		    cts?.Cancel();
		    cts?.Dispose();
		    cts = null;

		    connected = false;
	    }
		
		public void RegisterEvent(string eventName, Action<object> callback)
		{
			string eventNameAdjusted = eventName.ToLower();
			
			if (eventHandlers.ContainsKey(eventNameAdjusted))
			{
				eventHandlers[eventNameAdjusted] += callback;
			}
			else
			{
				eventHandlers[eventNameAdjusted] = callback;
			}
		}

		public void UnregisterEvent(string eventName, Action<object> callback)
		{
			string eventNameAdjusted = eventName.ToLower();
			
			if (eventHandlers.ContainsKey(eventNameAdjusted))
			{
				eventHandlers[eventNameAdjusted] -= callback;
				if (eventHandlers[eventNameAdjusted] == null)
				{
					eventHandlers.Remove(eventNameAdjusted);
				}
			}
		}

		private void OnApplicationQuit()
		{
			Disconnect();
		}
	}
}

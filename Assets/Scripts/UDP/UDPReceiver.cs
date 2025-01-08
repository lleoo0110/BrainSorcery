using System.Net;
using System.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class UDPReceiver : MonoBehaviour
{
    [Header("UDP設定")]
    [SerializeField] private int listenPort = 54321;
    [SerializeField] private bool showDebugLogs = true;

    [Header("イベント")]
    [SerializeField] public UnityEvent<EEGData> onEEGDataReceived;
    [SerializeField] private UnityEvent<string> onErrorOccurred;

    private UdpClient udpClient;
    private Queue<EEGData> dataQueue = new Queue<EEGData>();
    private bool isRunning = true;
    
    // 最新のデータを保持するプロパティ
    public EEGData LatestData { get; private set; }

    private void Start()
    {
        InitializeUDP();
    }

    private void InitializeUDP()
    {
        try
        {
            udpClient = new UdpClient(listenPort);
            if (showDebugLogs) Debug.Log($"UDP listener started on port {listenPort}");
            BeginReceive();
        }
        catch (Exception e)
        {
            Debug.LogError($"UDP initialization error: {e.Message}");
            onErrorOccurred?.Invoke($"初期化エラー: {e.Message}");
        }
    }

    private void BeginReceive()
    {
        if (!isRunning) return;
        try
        {
            udpClient.BeginReceive(OnReceived, null);
        }
        catch (Exception e)
        {
            Debug.LogError($"BeginReceive error: {e.Message}");
            onErrorOccurred?.Invoke($"受信エラー: {e.Message}");
        }
    }

    private void OnReceived(IAsyncResult result)
    {
        if (!isRunning) return;

        try
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, listenPort);
            byte[] receivedBytes = udpClient.EndReceive(result, ref remoteEP);

            string jsonData = Encoding.UTF8.GetString(receivedBytes);
            EEGData eegData = JsonUtility.FromJson<EEGData>(jsonData);

            if (eegData != null)
            {
                lock (dataQueue)
                {
                    dataQueue.Enqueue(eegData);
                }
            }

            BeginReceive();
        }
        catch (Exception e)
        {
            Debug.LogError($"OnReceived error: {e.Message}");
            onErrorOccurred?.Invoke($"データ処理エラー: {e.Message}");
            BeginReceive();
        }
    }

    private void Update()
    {
        lock (dataQueue)
        {
            while (dataQueue.Count > 0)
            {
                LatestData = dataQueue.Dequeue();
                onEEGDataReceived?.Invoke(LatestData);
            }
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
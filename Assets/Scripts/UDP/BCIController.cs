using UnityEngine;
using System.Collections.Generic;

public class BCIController : MonoBehaviour
{
    [System.Serializable]
    public class BCIInputMapping
    {
        public BCI_Input bciInput;
        public int targetLabel;
    }

    [Header("UDP Receiver")]
    [SerializeField] private UDPReceiver udpReceiver;

    [Header("Input Settings")]
    [SerializeField] private List<BCIInputMapping> inputMappings;

    private void Start()
    {
        if (udpReceiver == null)
        {
            Debug.LogError("UDPReceiver is not assigned!");
            return;
        }

        // UDPReceiverのイベントにリスナーを追加
        udpReceiver.onEEGDataReceived.AddListener(OnEEGDataReceived);
    }

    private void OnEEGDataReceived(EEGData data)
    {
        // 該当するラベルを持つ全てのBCI_Inputに対して実行
        foreach (var mapping in inputMappings)
        {
            if (mapping.targetLabel == data.prediction.label)
            {
                mapping.bciInput.ExecuteInput(data.prediction.label);
            }
        }
    }

    private void OnDestroy()
    {
        if (udpReceiver != null)
        {
            udpReceiver.onEEGDataReceived.RemoveListener(OnEEGDataReceived);
        }
    }
}
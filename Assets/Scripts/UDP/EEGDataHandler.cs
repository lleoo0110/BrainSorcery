using UnityEngine;
using UnityEngine.UI;

public class EEGDataHandler : MonoBehaviour
{
    // Prediction関連のUI要素
    [Header("Prediction")]
    [SerializeField] private Text predictionLabelText;     // 予測ラベル
    [SerializeField] private Text predictionScoreText;     // 予測スコア
    [SerializeField] private Text classifierText;          // 使用された分類器

    // Power特徴量のUI要素
    [Header("Power Features")]
    [SerializeField] private Text deltaText;               // δ波パワー
    [SerializeField] private Text thetaText;               // θ波パワー
    [SerializeField] private Text alphaText;               // α波パワー
    [SerializeField] private Text betaText;                // β波パワー
    [SerializeField] private Text gammaText;               // γ波パワー

    // FAA特徴量のUI要素
    [Header("FAA Features")]
    [SerializeField] private Text faaValueText;            // FAA値
    [SerializeField] private Text arousalStateText;        // 覚醒状態

    // α/β比のUI要素
    [Header("AB Ratio Features")]
    [SerializeField] private Text abRatioText;             // α/β比
    [SerializeField] private Text abStateText;             // 状態

    // 感情特徴量のUI要素
    [Header("Emotion Features")]
    [SerializeField] private Text emotionStateText;        // 感情状態
    [SerializeField] private Text emotionCoordsText;       // 感情座標
    [SerializeField] private Text emotionValuesText;       // 感情値配列

    public void OnEEGDataReceived(EEGData data)
    {
        // Prediction情報の更新
        if (data.prediction != null)
        {
            UpdateText(predictionLabelText, $"Label: {data.prediction.label}");
            UpdateText(predictionScoreText, $"Score: {data.prediction.score:F3}");
            UpdateText(classifierText, $"Classifier: {data.prediction.classifier}");
        }

        // Power特徴量の更新
        if (data.features?.power != null)
        {
            UpdateText(deltaText, $"Delta: {data.features.power.delta:F2}");
            UpdateText(thetaText, $"Theta: {data.features.power.theta:F2}");
            UpdateText(alphaText, $"Alpha: {data.features.power.alpha:F2}");
            UpdateText(betaText, $"Beta: {data.features.power.beta:F2}");
            UpdateText(gammaText, $"Gamma: {data.features.power.gamma:F2}");
        }

        // FAA特徴量の更新
        if (data.features?.faa != null)
        {
            UpdateText(faaValueText, $"FAA Value: {data.features.faa.faa:F3}");
            UpdateText(arousalStateText, $"Arousal: {data.features.faa.arousal}");
        }

        // α/β比の更新
        if (data.features?.abRatio != null)
        {
            UpdateText(abRatioText, $"AB Ratio: {data.features.abRatio.ratio:F3}");
            UpdateText(abStateText, $"State: {data.features.abRatio.state}");
        }

        // 感情特徴量の更新
        if (data.features?.emotion != null)
        {
            UpdateText(emotionStateText, $"Emotion: {data.features.emotion.state}");
            UpdateText(emotionCoordsText, $"Coordinates: ({data.features.emotion.coordinates.x:F2}, {data.features.emotion.coordinates.y:F2})");
            
            if (data.features.emotion.emotionCoords != null)
            {
                string emotionValues = "Emotion Values: ";
                for (int i = 0; i < data.features.emotion.emotionCoords.Length; i++)
                {
                    emotionValues += $"{data.features.emotion.emotionCoords[i]:F2} ";
                }
                UpdateText(emotionValuesText, emotionValues);
            }
        }

        // デバッグ出力
        PrintAllData(data);
    }

    // UI Text更新用のヘルパーメソッド
    private void UpdateText(Text textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
    }

    // デバッグ用のログ出力メソッド
    public void PrintAllData(EEGData data)
    {
        Debug.Log("=== EEG Data Debug Output ===");
        if (data.prediction != null)
        {
            Debug.Log($"Prediction - Label: {data.prediction.label}, " +
                     $"Score: {data.prediction.score}, " +
                     $"Classifier: {data.prediction.classifier}");
        }

        if (data.features != null)
        {
            if (data.features.power != null)
            {
                Debug.Log($"Power - Delta: {data.features.power.delta}, " +
                         $"Theta: {data.features.power.theta}, " +
                         $"Alpha: {data.features.power.alpha}, " +
                         $"Beta: {data.features.power.beta}, " +
                         $"Gamma: {data.features.power.gamma}");
            }

            if (data.features.faa != null)
            {
                Debug.Log($"FAA - Value: {data.features.faa.faa}, " +
                         $"Arousal: {data.features.faa.arousal}");
            }

            if (data.features.abRatio != null)
            {
                Debug.Log($"AB Ratio - Ratio: {data.features.abRatio.ratio}, " +
                         $"State: {data.features.abRatio.state}");
            }

            if (data.features.emotion != null)
            {
                Debug.Log($"Emotion - State: {data.features.emotion.state}, " +
                         $"Coordinates: ({data.features.emotion.coordinates.x}, " +
                         $"{data.features.emotion.coordinates.y})");
                
                if (data.features.emotion.emotionCoords != null)
                {
                    Debug.Log($"Emotion Values: {string.Join(", ", data.features.emotion.emotionCoords)}");
                }
            }
            Debug.Log("===========================");
        }
    }
}
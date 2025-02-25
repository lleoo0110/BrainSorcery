using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LogLecorder : MonoBehaviour
{
    [SerializeField] private CSVSaver CSVSaver = null;
    [SerializeField] [Tooltip("ログファイルのベース名 (BaseName_Timestamp.csvで保存されます)")]
    private string baseFileName = "GameLog"; // インスペクタで変更可能なデフォルト名

    private int eType = -1;
    private float knockOutTime = -1;
    private List<string> inputs = new List<string>();

    private string pathName = "";

    private void Start()
    {
        // タイムスタンプを生成 (YYYYMMDD_HHMMSS形式)
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        
        // 指定形式でファイル名を生成: [baseFileName]_[timestamp].csv
        string fileName = $"{baseFileName}_{timestamp}.csv";
        
        // フルパスを設定
        pathName = Path.Combine(Application.dataPath, "StreamingAssets", fileName);
    }

    public void SetEType(int eType)
    {
        this.eType = eType;
    }

    public void SetKnockOutTime(float knockOutTime)
    {
        this.knockOutTime = knockOutTime;
    }

    public void AddInput(int input)
    {
        inputs.Add(input.ToString());
    }

    public void Save()
    {
        //eType, knockOutTimeが-1でないか確認
        if (eType == -1 || knockOutTime == -1)
        {
            Debug.LogError("eType or knockOutTime is not set.");
            return;
        }

        //inputsが空でないか確認
        if (inputs.Count == 0)
        {
            Debug.LogError("inputs is empty.");
            return;
        }

        //ログを保存
        List<string> line = new List<string>();
        line.Add(eType.ToString());
        line.Add(knockOutTime.ToString());
        line.AddRange(inputs);

        Debug.Log(line);

        // ディレクトリが存在しない場合は作成
        string directory = Path.GetDirectoryName(pathName);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        CSVSaver.WriteCSV(pathName, line.ToArray());

        //初期化
        eType = -1;
        knockOutTime = -1;
        inputs.Clear();
    }
}
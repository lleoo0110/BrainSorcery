using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogLecorder : MonoBehaviour
{
    [SerializeField] private CSVSaver CSVSaver = null;

    private int eType = -1;
    private float knockOutTime = -1;
    private List<string> inputs = new List<string>();

    private string pathName = "";

    private void Start()
    {
        string nme =
            DateTime.Now.Month.ToString() + "-" + 
            DateTime.Now.Day.ToString() + "-" +
            DateTime.Now.Hour.ToString() + "-" +
            DateTime.Now.Minute.ToString() + "-" +
            DateTime.Now.Second.ToString();
        pathName = Application.dataPath + "/StreamingAssets/" + nme + ".csv";
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

        CSVSaver.WriteCSV(pathName, line.ToArray());

        //初期化
        eType = -1;
        knockOutTime = -1;
        inputs.Clear();
    }
}
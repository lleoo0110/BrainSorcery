using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CSVSaver : MonoBehaviour
{
    private string[] header = { "E-Type", "KnockDownT", "InputLog" };

    public void WriteCSV(string path, params string[] lineDatas)
    {
        // ファイルが存在するか確認
        bool fileExists = File.Exists(path);

        // ファイルストリームを使って書き込む/追記する
        using (StreamWriter writer = new StreamWriter(path, append: fileExists))
        {
            // ファイルが存在しない場合はヘッダーを書き込む
            if (!fileExists)
            {
                writer.WriteLine(string.Join(",", header));
            }

            // データを書き込む
            writer.WriteLine(string.Join(",", lineDatas));
        }

        Debug.Log("CSVファイルに書き込みが完了しました。パス: " + path);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // StringReader

public class CSVReader : BaseMonoBehaviour
{
    /// <summary>
    /// CSVファイル
    /// </summary>
    private TextAsset csvFile;

    /// <summary>
    /// CSVの中身を入れるリスト
    /// </summary>
    private List<string[]> csvDatas = new List<string[]>();
    public List<string[]> CsvDatas
    {
        get { return csvDatas; }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// CSVReader初期化
    /// </summary>
    private void Start()
    {
        csvFile = Resources.Load("test") as TextAsset; // Resouces下のCSV読み込み
        StringReader reader = new StringReader(csvFile.text);

        // , で分割しつつ一行ずつ読み込み
        // リストに追加していく
        while (reader.Peek() != -1) // reader.Peekが-1になるまで
        {
            string line = reader.ReadLine(); // 一行ずつ読み込み
            csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
        }

        // csvDatas[行][列]を指定して値を自由に取り出せる
        Debug.Log(csvDatas[0][1]);
    }
}

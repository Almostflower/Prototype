using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // StringReader

public class CSVReader : BaseMonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private TextAsset csvFile; // CSVファイル

    /// <summary>
    /// 
    /// </summary>
    private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    /// <returns></returns>
    public string GetData(int index1, int index2)
    {
        return csvDatas[index1][index2];
    }

    // 疑問
    // TextAssetはナニモン？
    // StringReaderはナニモン？
    // わざわざリストに入れてるけどTextAssetのままでは使えないの？
}

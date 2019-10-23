using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Stage : BaseMonoBehaviour
{
    /// <summary>
    /// オブジェクトタイプ
    /// </summary>
    public enum TypeObj
    {
        None = 0,
        Gift,
        Building,
    }

    /// <summary>
    /// エリアごとの構造体
    /// </summary>
    public struct Area
    {
        public GameObject obj;
        public Vector3 position;
        public bool isExistence;
        public TypeObj typeObj;
    }

    /// <summary>
    /// csvファイルのデータ
    /// </summary>
    [SerializeField] private GameObject csvData;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GameObject[] Gift;

    /// <summary> 幅サイズ </summary>
    [SerializeField] private int stageWidth;
    public int StageWidth
    {
        get { return stageWidth; }
    }

    /// <summary> 奥行サイズ </summary>
    [SerializeField] private int stageHeight;
    public int StageHeight
    {
        get { return stageHeight; }
    }

    /// <summary> エリアデータ </summary>
    private Area[,] stageArea; 
    public  Area StageArea(int index1, int index2)
    {
        return stageArea[index1, index2];
    }


    /// <summary>
    /// ステージの初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();        
    }

    private void Start()
    {

        // 生成エリア情報の初期化
        stageArea = new Area[stageHeight, stageWidth];
        int width = 2;
        Vector3 startPos = new Vector3(-stageWidth * width / 2, 0.5f, stageHeight * width / 2);
        for (int i = 0; i < stageHeight; i++)
        {
            for (int j = 0; j < stageWidth; j++)
            {
                float posX = startPos.x + j * width;
                float posY = startPos.y;
                float posZ = startPos.z - i * width;
                stageArea[i, j].position = new Vector3(posX, posY, posZ);
                stageArea[i, j].isExistence = false;
                stageArea[i, j].typeObj = TypeObj.None;

                //Debug.Log(csvData.GetComponent<CSVReader>().CsvDatas[0][0]);

                if (csvData.GetComponent<CSVReader>().CsvDatas[i][j] != Gift.Length.ToString())
                {
                    int index = int.Parse(csvData.GetComponent<CSVReader>().CsvDatas[i][j].ToString());

                    // 生成
                    stageArea[i, j].obj = Instantiate(Gift[index], stageArea[i, j].position, Quaternion.identity);
                    stageArea[i, j].isExistence = true;
                    stageArea[i, j].typeObj = TypeObj.Building;
                }

            }
        }
    }

    /// <summary>
    /// ステージの更新
    /// </summary>
    public override void UpdateNormal()
    {

    }
}

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
    [SerializeField] private GameObject Gift;

    /// <summary> 幅サイズ </summary>
    [SerializeField] private int stageWidth;

    /// <summary> 奥行サイズ </summary>
    [SerializeField] private int stageHeight;

    /// <summary> エリアデータ </summary>
    private Area[,] stageArea; 


    /// <summary>
    /// ステージの初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // 生成エリア情報の初期化
        stageArea = new Area[stageHeight, stageWidth];
        int width = 2;
        Vector3 startPos = new Vector3(-stageWidth * width / 2, 0.5f, stageHeight * width / 2);
        for (int i = 0; i < stageHeight; i++)
        {
            for(int j = 0; j < stageWidth; j++)
            {
                float posX = startPos.x + j * width;
                float posY = startPos.y;
                float posZ = startPos.z - i * width;
                stageArea[i, j].position = new Vector3(posX, posY, posZ);
                stageArea[i, j].isExistence = false;
                stageArea[i, j].typeObj = TypeObj.None;

                //Debug.Log(csvData.GetComponent<CSVReader>().CsvDatas[0][0]);

                if(csvData.GetComponent<CSVReader>().CsvDatas[i][j] == "1")
                {
                    // 生成
                    stageArea[i, j].obj = Instantiate(Gift, stageArea[i, j].position, Quaternion.identity);
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

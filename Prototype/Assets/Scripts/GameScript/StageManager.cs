using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public sealed class StageManager : BaseMonoBehaviour
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
    /// 建物
    /// </summary>
    [SerializeField] private GameObject[] Building;

    /// <summary> 幅サイズ </summary>
    [SerializeField, Tooltip("変更禁止")] private int stageWidth;
    public int StageWidth
    {
        get { return stageWidth; }
    }

    /// <summary> 奥行サイズ </summary>
    [SerializeField, Tooltip("変更禁止")] private int stageHeight;
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
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();        
    }

    /// <summary>
    /// ステージの初期化
    /// </summary>
    private void Start()
    {
        // csvからステージの幅高さを取得
        stageWidth = csvData.GetComponent<CSVReader>().StageWidth;
        stageHeight = csvData.GetComponent<CSVReader>().StageHeight;

        // 生成エリア情報の初期化
        stageArea = new Area[stageHeight, stageWidth];
        int width = 2;
        Vector3 startPos = new Vector3(-stageWidth * width / 2, 0.0f, stageHeight * width / 2);
        for (int i = 0; i < stageHeight; i++)
        {
            for (int j = 0; j < stageWidth; j++)
            {
                // オブジェクト配置の座標決定
                float posX = startPos.x + j * width;
                float posY = startPos.y;
                float posZ = startPos.z - i * width;
                stageArea[i, j].position = new Vector3(posX, posY, posZ);
                stageArea[i, j].isExistence = false;
                stageArea[i, j].typeObj = TypeObj.None;

                // CSVで読み込んだデータと比較して、建物かどうかを判定
                if (csvData.GetComponent<CSVReader>().CsvDatas[i][j] != Building.Length.ToString())
                {
                    int index = int.Parse(csvData.GetComponent<CSVReader>().CsvDatas[i][j].ToString());

                    // 生成
                    stageArea[i, j].obj = Instantiate(Building[index], stageArea[i, j].position, Quaternion.identity);
                    stageArea[i, j].obj.transform.parent = this.transform;
                    stageArea[i, j].isExistence = true;
                    stageArea[i, j].typeObj = TypeObj.Building;
                }

            }
        }

        GetComponent<NavMeshSurface> ().BuildNavMesh();
    }

    /// <summary>
    /// ステージの更新
    /// </summary>
    public override void UpdateNormal()
    {

    }
}

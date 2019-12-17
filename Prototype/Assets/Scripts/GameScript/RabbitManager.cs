using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : BaseMonoBehaviour
{
    public enum RabbitType
    {
        None = -1,
        Good = 0,
        Bad = 1,
    };

    /// <summary>
    /// プレイヤーprefab
    /// </summary>
    [SerializeField] private GameObject player;

    /// <summary>
    /// ウサギのprefab
    /// </summary>
    [SerializeField] private GameObject[] rabbitData;

    /// <summary>
    /// ステージのデータ
    /// </summary>
    [SerializeField] private GameObject stageManager;

    /// <summary>
    /// ウサギ（良い）最大値
    /// </summary>
    [SerializeField, Tooltip("ウサギ(良)の最大値（1-2）")] private int goodRabbitMax;

    /// <summary>
    /// ウサギ（悪い）最大値
    /// </summary>
    [SerializeField, Tooltip("ウサギ(悪)の最大値（1-2）")] private int badRabbitMax;

    /// <summary>
    /// ウサギの総数
    /// </summary>
    private int rabbitMaxNum;
    public int RabbitMaxNum
    {
        get { return rabbitMaxNum; }
    }

    /// <summary>
    /// ウサギの生成オブジェクト
    /// </summary>
    public GameObject[] rabbitManager;
    public bool[] isExistence;
    public RabbitType[] rabbitType;


    /// <summary>
    /// 空白ステージの座標
    /// </summary>
    private List<Vector3> rabbitArea;

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ウサギマネージャーの初期化
    /// </summary>
    private void Start()
    {
        // ウサギの総数を求める
        rabbitMaxNum = goodRabbitMax + badRabbitMax;

        // ウサギのメモリ確保
        rabbitManager = new GameObject[rabbitMaxNum];
        isExistence = new bool[rabbitMaxNum];
        rabbitType = new RabbitType[rabbitMaxNum];

        // ウサギ生成可能座標のメモリ確保
        rabbitArea = new List<Vector3>();

        // ステージ空白の配列の生成
        for (int i = 0; i < stageManager.GetComponent<StageManager>().StageHeight; i++)
        {
            for (int j = 0; j < stageManager.GetComponent<StageManager>().StageWidth; j++)
            {
                if (!stageManager.GetComponent<StageManager>().StageArea(i, j).isExistence)
                {
                    rabbitArea.Add(stageManager.GetComponent<StageManager>().StageArea(i, j).position);
                }

            }
        }

        // ウサギの生成
        for (int i = 0; i < rabbitMaxNum; i++)
        {
            Birth(i);
        }

    }

    /// <summary>
    /// ウサギマネージャー更新
    /// </summary>
    public override void UpdateNormal()
    {
        // ウサギの生存チェック
        for (int i = 0; i < rabbitMaxNum; i++)
        {
            if (isExistence[i])
            {
                // ウサギのステートがDEADかチェック
                if (rabbitManager[i].GetComponent<RabbitScript>().sCurrentState == RabbitScript.RabbitState.DEAD)
                {
                    // ウサギの削除と生成
                    Delete(i);
                }

            }

            if(!isExistence[i])
            {
                playBirth(i);
            }
        }
    }

    /// <summary>
    /// リストから一つランダムに取得する
    /// </summary>
    /// <returns>座標を取得する</returns>
    public Vector3 GetPositionFromList()
    {
        int index = UnityEngine.Random.Range(0, rabbitArea.Count);
        Vector3 target = rabbitArea[index];
        rabbitArea.RemoveAt(index);
        target.y = 0.5f;

        return target;
    }

    /// <summary>
    /// ウサギ生成可能エリアを追加する
    /// </summary>
    /// <param name="pos"></param>
    public void AddListToRabbitArea(Vector3 pos)
    {
        rabbitArea.Add(pos);
    }

    /// <summary>
    /// ウサギの削除
    /// </summary>
    /// <param name="index"></param>
    private void Delete(int index)
    {
        // 削除
        AddListToRabbitArea(rabbitManager[index].transform.position);
        //Destroy(rabbitManager[index]);
        rabbitManager[index].SetActive(false);
        isExistence[index] = false;
    }

    /// <summary>
    /// ウサギの生成(Startのみ)
    /// </summary>
    /// <param name="index"></param>
    private void Birth(int index)
    {
        if (index < goodRabbitMax)
        {
            rabbitManager[index] = Instantiate(rabbitData[(int)RabbitType.Good], GetPositionFromList(), Quaternion.identity);
            rabbitType[index] = RabbitType.Good;
        }
        else
        {
            rabbitManager[index] = Instantiate(rabbitData[(int)RabbitType.Bad], GetPositionFromList(), Quaternion.identity);
            rabbitType[index] = RabbitType.Bad;
        }

        rabbitManager[index].transform.parent = this.transform;
        isExistence[index] = true;
    }

    /// <summary>
    /// ウサギの生成
    /// </summary>
    /// <param name="index"></param>
    private void playBirth(int index)
    {

        // ウサギを初期化
        rabbitManager[index].SetActive(true);
        rabbitManager[index].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.ORDINARY;

        if (index < goodRabbitMax)
        {
            rabbitManager[index].transform.position = GetPositionFromList();
            //rabbitManager[index] = Instantiate(rabbitData[(int)RabbitType.Good], GetPositionFromList(), Quaternion.identity);
            rabbitType[index] = RabbitType.Good;
        }
        else
        {
            rabbitManager[index].transform.position = GetPositionFromList();
            //rabbitManager[index] = Instantiate(rabbitData[(int)RabbitType.Bad], GetPositionFromList(), Quaternion.identity);
            rabbitType[index] = RabbitType.Bad;
        }

        rabbitManager[index].GetComponent<RabbitScript>().HitPlayer = false;
        rabbitManager[index].GetComponent<RabbitScript>().rabbitCircleReset().ResetCircle();
        rabbitManager[index].transform.parent = this.transform;
        isExistence[index] = true;
    }
}

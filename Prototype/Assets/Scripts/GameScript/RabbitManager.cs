using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : BaseMonoBehaviour
{
    enum RabbitType
    {
        None = -1,
        Good = 0,
        Bad = 1,
    };


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
    /// ウサギの生成オブジェクト
    /// </summary>
    private GameObject[] rabbitManager;
    private bool[] isExistence;
    private RabbitType[] rabbitType;


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
        // ウサギのメモリ確保
        rabbitManager = new GameObject[goodRabbitMax + badRabbitMax];
        isExistence = new bool[goodRabbitMax + badRabbitMax];
        rabbitType = new RabbitType[goodRabbitMax + badRabbitMax];

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
        for (int i = 0; i < goodRabbitMax + badRabbitMax; i++)
        {
            if(i < goodRabbitMax)
            {
                Birth(i, RabbitType.Good);
            }
            else
            {
                Birth(i, RabbitType.Bad);
            }
            
        }


    }

    /// <summary>
    /// ウサギマネージャー更新
    /// </summary>
    public override void UpdateNormal()
    {
        // ウサギの生存チェック
        for (int i = 0; i < goodRabbitMax + badRabbitMax; i++)
        {
            if (isExistence[i])
            {
                //// 自然消滅
                //if (rabbitManager[i].GetComponent<Gift>().GetDeathFlag())
                //{
                //    // ステージゲージを下げる
                //
                //    // ギフトの削除と生成
                //    Delete(i);
                //    Birth(i);
                //
                //}
                //
                //// 良い状態で回収
                //if (giftManager[i].GetComponent<Gift>().GoodFlag)
                //{
                //    // ステージゲージを上げる
                //
                //    // ギフトの削除と生成
                //    Delete(i);
                //    Birth(i);
                //}

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
        Destroy(rabbitManager[index]);
        isExistence[index] = false;
    }

    /// <summary>
    /// ウサギの生成
    /// </summary>
    /// <param name="index"></param>
    private void Birth(int index, RabbitType type)
    {
        rabbitManager[index] = Instantiate(rabbitData[(int)type], GetPositionFromList(), Quaternion.identity);
        rabbitManager[index].transform.parent = this.transform;
        isExistence[index] = true;
        rabbitType[index] = type;
    }
}

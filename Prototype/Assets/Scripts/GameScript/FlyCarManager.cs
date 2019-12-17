using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCarManager : BaseMonoBehaviour
{
    /// <summary>
    /// 構造体
    /// </summary>
    struct FLY_CAR
    {
        public GameObject obj;
        public bool isFlag;
        public int root;
    };

    /// <summary>
    /// 車のプレファブ
    /// </summary>
    [SerializeField]private GameObject flyCar;

    /// <summary>
    /// 車の管理者
    /// </summary>
    private FLY_CAR[] flyCars;

    /// <summary>
    /// 生成数
    /// </summary>
    [SerializeField]private int maxNum;
    [SerializeField] private Vector3[] startPos;
    [SerializeField] private Vector3[] endPos;

    /// <summary>
    /// 走るルートの管理
    /// </summary>
    [SerializeField]private List<int> root;

    private float ReStartTime;
    protected override void Awake()
    {
        base.Awake();
    }


    // Start is called before the first frame update
    void Start()
    {
        ReStartTime = 0.0f;

        flyCars = new FLY_CAR[maxNum];

        // 車生成可能座標のメモリ確保
        root = new List<int>();

        for (int i = 0; i < maxNum; i++)
        {
            flyCars[i].obj = Instantiate(flyCar);
            flyCars[i].isFlag = false;
        }

        // 車のルートを追加
        for(int i = 0; i < startPos.Length; i++)
        {
            AddListToRoot(i);
        }
    }

    public override void UpdateNormal()
    {
        ReStartTime += Time.deltaTime;

        // 車を生成させる
        for (int i = 0; i < maxNum; i++)
        {
            // 
            if(ReStartTime > 3.0f)
            {
                if (!flyCars[i].isFlag)
                {
                    flyCars[i].obj.SetActive(true);
                    flyCars[i].obj.transform.position = startPos[flyCars[i].root];
                    flyCars[i].obj.GetComponent<FlyCar>().ReStart();
                    flyCars[i].root = GetRootFromList();
                    flyCars[i].obj.GetComponent<FlyCar>().StartPos = startPos[flyCars[i].root];
                    flyCars[i].obj.GetComponent<FlyCar>().EndPos = endPos[flyCars[i].root];
                    flyCars[i].isFlag = true;
                    flyCars[i].obj.transform.parent = this.transform;

                }

                // 最終地点に車が到達したかチェック
                if (flyCars[i].obj.GetComponent<FlyCar>().IsDead)
                {

                    AddListToRoot(flyCars[i].root);
                    //                Destroy(flyCars[i].obj);
                    flyCars[i].obj.SetActive(false);
                    flyCars[i].isFlag = false;
                }
            }
        }

        if(ReStartTime > 4.0f)
        {
            ReStartTime = 0.0f;
        }
    }

    /// <summary>
    /// リストから一つランダムに取得する
    /// </summary>
    /// <returns>座標を取得する</returns>
    public int GetRootFromList()
    {
        int index = UnityEngine.Random.Range(0, root.Count);
        int target = root[index];
        root.RemoveAt(index);

        return target;
    }

    /// <summary>
    /// ギフト生成可能エリアを追加する
    /// </summary>
    /// <param name="pos"></param>
    public void AddListToRoot(int index)
    {
        root.Add(index);
    }
}

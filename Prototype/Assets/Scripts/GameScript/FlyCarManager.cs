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


    protected override void Awake()
    {
        base.Awake();
    }


    // Start is called before the first frame update
    void Start()
    {
        flyCars = new FLY_CAR[maxNum];

        for(int i = 0; i < maxNum; i++)
        {
            flyCars[i].isFlag = false;
        }
    }

    public override void UpdateNormal()
    {
        // 車を生成させる
        for (int i = 0; i < maxNum; i++)
        {
            // 
            if (!flyCars[i].isFlag)
            {
                flyCars[i].obj = Instantiate(flyCar);
                flyCars[i].obj.GetComponent<FlyCar>().StartPos = startPos[i];
                flyCars[i].obj.GetComponent<FlyCar>().EndPos = endPos[i];
                flyCars[i].isFlag = true;
            }

            // 最終地点に車が到達したかチェック
            if(flyCars[i].obj.GetComponent<FlyCar>().IsDead)
            {
                Destroy(flyCars[i].obj);
                flyCars[i].isFlag = false;
            }
        }
    }
}

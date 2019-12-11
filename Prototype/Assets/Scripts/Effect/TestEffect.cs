using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : BaseMonoBehaviour
{
    public struct objData
    {
        public GameObject obj;
        public bool existenceFlag;
        public float destroyTime;
    };

    /// <summary>
    /// パーティクルで使用するパーツ
    /// </summary>
    [SerializeField, Tooltip("使用するオブジェクト")] private GameObject cube;
    [SerializeField, Tooltip("生成オブジェクトの個数制限")] private int limitObjNum;
    [SerializeField, Tooltip("生成間隔時間")] private float sponeTime;
    [SerializeField, Tooltip("生成間隔時間ごとの生成個数")] private float sponeNum;
    [SerializeField, Tooltip("生成されてから消えるまでの時間")] private float destroyTime;
    [SerializeField, Tooltip("生成されているオブジェクトの数")] private int objNum;
    [SerializeField, Tooltip("生成位置X")] private Vector2 rangeWidth;
    [SerializeField, Tooltip("生成位置Y")] private Vector2 rangeHeight;
    [SerializeField, Tooltip("生成位置Z")] private Vector2 rangeDepth;

    private objData[] objs;
    private float sponeCopyTime;
    

    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        sponeCopyTime = sponeTime;
        objs = new objData[limitObjNum];
        for(int i =0;i < limitObjNum; i++)
        {
            objs[i].obj = null;
            objs[i].existenceFlag = false;
            objs[i].destroyTime = 0;
            objNum = 0;
        }
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        float deltaTime = Time.deltaTime;
        sponeCopyTime -= deltaTime;

        // 発生
        if(sponeCopyTime <= 0.0f)
        {
            int copyObjNum = objNum;
            for(int i = 0; i < limitObjNum; i++)
            {
                if ((objNum - copyObjNum) >= sponeNum) { break; }
                if (!objs[i].existenceFlag)
                {
                    objs[i].obj = Instantiate(cube, new Vector3(Random.Range(rangeWidth.x, rangeWidth.y), Random.Range(rangeHeight.x, rangeHeight.y), Random.Range(rangeDepth.x, rangeDepth.y)), Quaternion.identity);
                    objs[i].existenceFlag = true;
                    objs[i].destroyTime = destroyTime;
                    objNum++;
                    objs[i].obj.transform.parent = this.transform;
                }
            }
            sponeCopyTime = sponeTime;
        }

        // 削除
        for (int i = 0; i < limitObjNum; i++)
        {
            // タイムを進める
            objs[i].destroyTime -= deltaTime;

            if (objs[i].existenceFlag)
            {
                if(objs[i].destroyTime <= 0.0f)
                {
                    objs[i].existenceFlag = false;
                    objs[i].destroyTime = 0.0f;
                    Destroy(objs[i].obj);
                    objs[i].obj = null;
                    objNum--;
                }
                
            }
        }

        //Debug.Log(objs[0].destroyTime);
    }
}

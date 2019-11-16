using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEffect : BaseMonoBehaviour
{
    /// <summary>
    /// パーティクルで使用するパーツ
    /// </summary>
    [SerializeField, Tooltip("使用するオブジェクト")] private GameObject cube;

    [SerializeField, Tooltip("生成間隔時間")] private float sponeTime;
    [SerializeField, Tooltip("生成間隔時間ごとの生成個数")] private float sponeNum;
    [SerializeField, Tooltip("生成されてから消えるまでの時間")] private float destroyTime;
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
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        sponeCopyTime -= Time.deltaTime;

        // 発生
        if(sponeCopyTime <= 0.0f)
        {
            for(int i = 0; i < sponeNum; i++)
            {
                GameObject maborosi;
                maborosi = Instantiate(cube, new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-2.0f, 0.0f), 0.0f), Quaternion.identity);
                Destroy(maborosi, destroyTime);
                sponeCopyTime = sponeTime;
            }
            
        }
    }
}

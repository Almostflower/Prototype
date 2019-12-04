using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCar : BaseMonoBehaviour
{
    /// <summary>
    /// 移動開始位置
    /// </summary>
    [SerializeField]private Vector3 startPos;

    /// <summary>
    /// 移動終了位置
    /// </summary>
    [SerializeField]private Vector3 endPos;

    private float distance;

    /// <summary>
    /// 終了位置までにかかる時間
    /// </summary>
    [SerializeField]private float time;

    private float counter = 0;


    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 二点間の距離をとる
        distance = Vector3.Distance(startPos, endPos);

        // 進む方向に向かせる
        Vector3 front = endPos - startPos;
        this.transform.forward = front.normalized;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void UpdateNormal()
    {
        counter += Time.deltaTime;
        float currentPos = (counter / time) * distance;

        // 座標を動かす
        this.transform.position = Vector3.Lerp(startPos, endPos, currentPos);

        // 削除
        if(this.transform.position == endPos)
        {
            Destroy(this.gameObject);
        }

        Debug.Log(counter);
    }
}

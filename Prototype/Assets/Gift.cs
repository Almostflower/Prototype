using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : BaseMonoBehaviour
{
    /// <summary>
    /// 不良品になるまでのリミットタイム
    /// </summary>
    [SerializeField]
    private float BadLimitTime = 10.0f;

    /// <summary>
    /// 消えるまでのリミットタイム
    /// </summary>
    [SerializeField]
    private float DustLimitTime = 10.0f;

    /// <summary>
    /// ギフトが粗悪になってある一定時間すぎた時　true
    /// </summary>
    private bool DustFlag;

    /// <summary>
    /// 自然消滅
    /// </summary>
    private bool DeathFlag;

    /// <summary>
    /// ギフトがいい状態で回収されたかチェック
    /// </summary>
    private bool goodFlag;
    public bool GoodFlag
    {
        get { return goodFlag; }
        set { goodFlag = value; }
    }

    /// <summary>
    /// プレイヤーがギフトを運んでいるかのチェック
    /// </summary>
    private bool playerCarryFlag;
    public bool PlayerCarryFlag
    {
        get { return playerCarryFlag; }
        set { playerCarryFlag = value; }
    }

    /// <summary>
    /// BaseMonoBehaviourの初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ギフトの初期化
    /// </summary>
    void Start()
    {
        DustFlag = false;
        DeathFlag = false;
        PlayerCarryFlag = false;
    }

    /// <summary>
    /// ギフトの更新
    /// </summary>
    public override void UpdateNormal()
    {
 
        if (!DustFlag && BadLimitTime > 0.0f)
        {
            // ギフトの良い状態の更新
            BadLimitTime -= Time.deltaTime;
        }
        else if (DustFlag && !playerCarryFlag)
        {
            // ギフトの悪い状態の更新
            DustLimitTime -= Time.deltaTime;
        }

        // 良い状態から悪い状態への条件判定
        if (BadLimitTime < 0.0f)
        {
            if (gameObject.tag != "Bad gift")
            {
                gameObject.tag = "Bad gift";
                Debug.Log("ギフトが悪くなった");
                this.GetComponent<Renderer>().material.color = Color.red;
            }

            DustFlag = true;
        }

        // 悪い状態から自然消滅への条件判定
        if (/*!GameStatusManager.Instance.GetLiftGift() && */DustFlag && DustLimitTime < 0.0f)
        {
            DeathFlag = true;
            Debug.Log("ギフト消滅");
        }
    }

    /// <summary>
    /// ギフトの状態が悪いか判定
    /// </summary>
    /// <returns></returns>
    public bool GetDustFlag()
    {
        return DustFlag;
    }

    /// <summary>
    /// ギフトの状態が消滅していいか判定
    /// </summary>
    public bool GetDeathFlag()
    {
        return DeathFlag;
    }

}
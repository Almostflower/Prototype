using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGift : BaseMonoBehaviour
{
    [SerializeField]
    private float BadLimitTime = 10.0f; //不良品になるまでのリミットタイム
    [SerializeField]
    private float DustLimitTime = 20.0f; //消えるまでのリミットタイム

    private bool DustFlag;  //ギフトが粗悪になってある一定時間すぎた時　true
    private bool DeathFlag;
    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        DustFlag = false;
        DeathFlag = false;
    }

    // Update is called once per frame
     public override void UpdateNormal()
    {
        if(!DustFlag && BadLimitTime > 0.0f)
        {
            BadLimitTime -= Time.deltaTime;
        }
        else if(DustFlag)
        {
            DustLimitTime -= Time.deltaTime;
        }

        if(BadLimitTime < 0.0f)
        {
            if(gameObject.tag != "Bad gift")
            {
                gameObject.tag = "Bad gift";
            }

            DustFlag = true;
        }

        if(!GameStatusManager.Instance.GetLiftGift() && DustFlag && DustLimitTime < 0.0f)
        {
            DeathFlag = true;
        }
    }

    public bool GetDustFlag()
    {
        return DustFlag;
    }

    public bool GetDeathFlag()
    {
        return DeathFlag;
    }

}

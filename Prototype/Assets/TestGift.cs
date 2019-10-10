using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGift : MonoBehaviour
{
    [SerializeField]
    private float BadLimitTime = 10.0f; //不良品になるまでのリミットタイム
    [SerializeField]
    private float DustLimitTime = 20.0f; //消えるまでのリミットタイム

    private bool BadFlag;
    private bool DustFlag;
    // Start is called before the first frame update
    void Start()
    {
        BadFlag = false;
        DustFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!BadFlag)
        {
            BadLimitTime -= Time.deltaTime;
        }
        else if(BadFlag && !DustFlag)
        {
            DustLimitTime -= Time.deltaTime;
        }

        if(BadLimitTime < 0.0f)
        {
            gameObject.tag = "Bad gift";
        }

        if(DustLimitTime < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}

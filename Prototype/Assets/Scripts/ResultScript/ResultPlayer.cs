using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private bool ActionAnimFlag;
    private int Actionnum;
    public bool ActionAnim
    {
        get { return ActionAnimFlag; }
        set { ActionAnimFlag = value; }
    }

    public int ActionNumber
    {
        get { return Actionnum; }
        set { Actionnum = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.transform.rotation.y);
        gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //220
        if(gameObject.transform.position.x > 418.4)
        {
            anim.SetBool("Walk", true);
            gameObject.transform.position += new Vector3(-0.1f, 0.0f, 0.0f);
        }
        else
        {
            //正面向く
            //if (gameObject.transform.rotation.y <= 0.99f)
            //{
            //    Debug.Log(gameObject.transform.rotation.y);
            //    gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -1.0f);
            //}
            //else
            //{
            //    anim.SetBool("Walk", false);
            //    anim.SetBool("Wait", true);
            //}
            //カメラのほう向く
            if(gameObject.transform.rotation.y <= 0.93f)
            {
                gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -1.0f);
            }
            else
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Wait", true);
            }
        }

        //スコアに対するアクション
        //if(ActionAnim)
        //{
        //    switch(ActionNumber)
        //    {
        //        case 0:
        //            anim.SetBool("Wait", true);
        //            break;
        //        case 1:
        //            anim.SetBool("Wait", true);
        //            break;
        //        case 2:
        //            anim.SetBool("Wait", true);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
}

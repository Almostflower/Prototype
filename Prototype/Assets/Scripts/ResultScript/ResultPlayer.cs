using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private bool ActionAnimFlag;
    private int Actionnum;
    private bool JudgeFlag;

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
        JudgeFlag = false;
        Debug.Log(gameObject.transform.rotation.y);
        gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), 90.0f);
    }

    // Update is called once per frame
    void Update()
    {

        if (JudgeFlag)
        {
            anim.SetBool("Wait", false);

            switch (SceneStatusManager.Instance.JudgeType)
            {
                case 0:
                    break;
                case 1:
                    anim.SetBool("Bad", true);
                    break;
                case 2:
                    anim.SetBool("Normal", true);
                    break;
                case 3:
                    anim.SetBool("Good", true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            //220
            if (gameObject.transform.position.x > 418.4f)
            {
                anim.SetBool("Walk", true);
                gameObject.transform.position += new Vector3(-0.2f, 0.0f, 0.0f);
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
                if (gameObject.transform.rotation.y <= 0.93f)
                {
                    gameObject.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), -1.0f);
                }
                else
                {
                    anim.SetBool("Walk", false);
                    anim.SetBool("Wait", true);
                    JudgeFlag = true;
                }
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

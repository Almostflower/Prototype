using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialTimeState : MonoBehaviour
{
    public enum THUTORIALSTATE
    {
        GOOD_GIFT = 0,
        GOOD_RABBIT,
        BAD_GIFT,
        BAD_RABBIT,
        DASH,
        WARP,
        NONE,
    }
    [SerializeField]
    private float[] settimes = new float[6];    //説明画像を切り替える時間用の変数
    
    private float nowTime;//現在時刻

    [SerializeField]
    private GameObject GoodGiftObj;
    [SerializeField]
    private GameObject BadGiftObj;
    [SerializeField]
    private GameObject GoodRabbitObj;
    [SerializeField]
    private GameObject BadRabbitObj;


    [SerializeField]
    private GameObject[] tutorialtex;
    private void Awake()
    {
        GoodGiftObj = Instantiate(GoodGiftObj, new Vector3(0.0f, 0.5f, 10.0f), Quaternion.identity);
        BadGiftObj = Instantiate(BadGiftObj, new Vector3(0.0f, 0.5f, 10.0f), Quaternion.identity);
        GoodRabbitObj = Instantiate(GoodRabbitObj, new Vector3(0.0f, 0.5f, 15.0f), Quaternion.identity);
        BadRabbitObj = Instantiate(BadRabbitObj, new Vector3(0.0f, 0.5f, 15.0f), Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        GoodGiftObj.SetActive(false);
        BadGiftObj.SetActive(false);
        GoodRabbitObj.SetActive(false);
        BadRabbitObj.SetActive(false);
        nowTime = 0.0f;
    }
    //TutorialManagerScript.Instance.SetPhaseCheck(false); //フェードアウトさせるときにこれを呼ぶ！
    //TutorialManagerScript.Instance.SetTimeCheckFlag(false); //Update内の時間を止める

    // Update is called once per frame

    int statecount0 = 0;
    int statecount1 = 3;
    int statecount2 = 6;
    int statecount3 = 8;
    int statecount4 = 10;
    int statecount5 = 11;
    int statecount6 = 12;
    int statecount7 = 13;
    int statecount8;
    int statecount9;
    int statecount10;
    int statecount11;
    int statecount12;
    int statecount13;
    void Update()
    {
        if(TutorialManagerScript.Instance.GetTimeCheckFlag() == true)
        {
            TutorialManagerScript.Instance.SetPhaseCheck(true);
            nowTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                switch(TutorialManagerScript.Instance.GetPhaseNumber())
                {
                    case 0:
                        statecount0++;
                        break;
                    case 1:
                        statecount1++;
                        break;
                    case 2:
                        statecount2++;
                        break;
                    case 3:
                        statecount3++;
                        break;
                    case 4:
                        statecount4++;
                        break;
                    case 5:
                        statecount5++;
                        break;
                    case 6:
                        statecount6++;
                        break;
                    case 7:
                        statecount7++;
                        break;
                    case 8:
                        statecount8++;
                        break;
                    case 9:
                        statecount9++;
                        break;
                    case 10:
                        statecount10++;
                        break;
                    case 11:
                        statecount11++;
                        break;
                    case 12:
                        statecount12++;
                        break;
                    case 13:
                        statecount13++;
                        break;
                }
            }
            switch (TutorialManagerScript.Instance.GetPhaseNumber())
            {
                case 0:
                    if (nowTime >= settimes[0])
                    {
                        nowTime = 0.0f;

                        if(statecount0 < 3)
                        {
                            ActiveImage(statecount0);
                        }
                        GoodGiftObj.SetActive(true);
                    }
                    break;
                case 1:
                    if (nowTime >= settimes[1])
                    {
                        nowTime = 0.0f;
                        if (statecount1 < 6)
                        {
                            ActiveImage(statecount1);
                        }
                        GoodRabbitObj.SetActive(true);
                    }
                    break;
                case 2:
                    if (nowTime >= settimes[2])
                    {
                        nowTime = 0.0f;
                        if(statecount2 < 8)
                        {
                            ActiveImage(statecount2);
                        }
                        BadGiftObj.SetActive(true);
                    }
                    break;
                case 3:
                    if (nowTime >= settimes[3])
                    {
                        nowTime = 0.0f;
                        if (statecount3 < 10)
                        {
                            ActiveImage(statecount3);
                        }
                        BadRabbitObj.SetActive(true);
                    }
                    break;
                case 4:
                    if (nowTime >= settimes[4])
                    {
                        if (statecount4 < 11)
                        {
                            ActiveImage(statecount4);
                        }
                        else
                        {
                            TutorialManagerScript.Instance.SetPhaseNumber(5);
                        }
                        nowTime = 0.0f;
                    }
                    break;
                case 5:
                    if (nowTime >= settimes[5])
                    {
                        if (statecount5 < 12)
                        {
                            ActiveImage(statecount5);
                        }

                        nowTime = 0.0f;
                    }
                    break;
                case 6:
                    if(nowTime >= settimes[6])
                    {
                        if (statecount6 < 13)
                        {
                            ActiveImage(statecount6);
                        }

                        nowTime = 0.0f;
                    }
                    break;
                case 7:
                    if (nowTime >= settimes[7])
                    {
                        if (statecount7 < 14)
                        {
                            ActiveImage(statecount7);
                        }

                        if(Input.GetKeyDown(KeyCode.Space))
                        {
                            ResetImage();
                        }
                        nowTime = 0.0f;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            nowTime = 0.0f;
        }
    }

    private void ActiveImage(int num)
    {
        for (int i = 0; i < 14; i++)
        {
            tutorialtex[i].SetActive(false);
        }

        tutorialtex[num].SetActive(true);
    }

    private void ResetImage()
    {
        for (int i = 0; i < 14; i++)
        {
            tutorialtex[i].SetActive(false);
        }
    }
}

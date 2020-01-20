using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private Transform point1, point2, point3, point4;

    [SerializeField]
    private GameObject[] tutorialtex;

    private void Awake()
    {
        GoodGiftObj = Instantiate(GoodGiftObj, point1.position, Quaternion.identity);
        BadGiftObj = Instantiate(BadGiftObj, point2.position, Quaternion.identity);
        GoodRabbitObj = Instantiate(GoodRabbitObj, point3.position, Quaternion.identity);
        BadRabbitObj = Instantiate(BadRabbitObj, point4.position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        SceneStatusManager.Instance.SetFadeOut(true);
        SceneStatusManager.Instance.SetFadeIn(false);
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
    int statecount7 = 14;

    float resettime = 0.0f;
    bool setumeiflag = false;
    void Update()
    {
        if(setumeiflag)
        {
            resettime += Time.deltaTime;

            if(resettime > 0.5f)
            {
                setumeiflag = false;
                resettime = 0.0f;
            }
        }
        if(TutorialManagerScript.Instance.GetTimeCheckFlag() == true)
        {
            TutorialManagerScript.Instance.SetPhaseCheck(true);
            nowTime += Time.deltaTime;

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0) && !setumeiflag)
            {
                setumeiflag = true;
                switch (TutorialManagerScript.Instance.GetPhaseNumber())
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

                        SceneStatusManager.Instance.TutorialWarp = true;
                        nowTime = 0.0f;
                    }
                    break;
                case 6:
                    if(nowTime >= settimes[6])
                    {
                        if (statecount6 < 14)
                        {
                            ActiveImage(statecount6);
                        }

                        nowTime = 0.0f;
                    }
                    break;
                case 7:
                    if (nowTime >= settimes[7])
                    {
                        if (statecount7 < 15)
                        {
                            ActiveImage(statecount7);

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                ResetImage();
                                SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Decision_SE);

                                SceneStatusManager.Instance.SetFadeIn(false);
                                SceneStatusManager.Instance.SetFadeIn(true);
                                //SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
                            }
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
        for (int i = 0; i < 15; i++)
        {
            tutorialtex[i].SetActive(false);
        }

        tutorialtex[num].SetActive(true);
    }

    private void ResetImage()
    {
        for (int i = 0; i < 15; i++)
        {
            tutorialtex[i].SetActive(false);
        }
    }
}

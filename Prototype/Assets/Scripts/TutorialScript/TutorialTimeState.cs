using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Update()
    {
        if(TutorialManagerScript.Instance.GetTimeCheckFlag() == true)
        {
            TutorialManagerScript.Instance.SetPhaseCheck(true);
            nowTime += Time.deltaTime;

            switch (TutorialManagerScript.Instance.GetPhaseNumber())
            {
                case 0:
                    if (nowTime >= settimes[0])
                    {
                        nowTime = 0.0f;
                        GoodGiftObj.SetActive(true);
                    }
                    break;
                case 1:
                    if (nowTime >= settimes[1])
                    {
                        nowTime = 0.0f;
                        GoodRabbitObj.SetActive(true);
                    }
                    break;
                case 2:
                    if (nowTime >= settimes[2])
                    {
                        nowTime = 0.0f;
                        BadGiftObj.SetActive(true);
                    }
                    break;
                case 3:
                    if (nowTime >= settimes[3])
                    {
                        nowTime = 0.0f;
                        BadRabbitObj.SetActive(true);
                    }
                    break;
                case 4:
                    if (nowTime >= settimes[4])
                    {
                        nowTime = 0.0f;
                    }
                    break;
                case 5:
                    if (nowTime >= settimes[5])
                    {
                        nowTime = 0.0f;
                    }
                    break;
            }
        }
        else
        {
            nowTime = 0.0f;
        }
    }
}

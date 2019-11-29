using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTimeState : MonoBehaviour
{
    [SerializeField]
    private float[] settimes = new float[6];    //説明画像を切り替える時間用の変数
    
    private float nowTime;//現在時刻


    // Start is called before the first frame update
    void Start()
    {
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
        }
        else
        {
            nowTime = 0.0f;
        }

        switch(TutorialManagerScript.Instance.GetPhaseNumber())
        {
            case 0:
                if (nowTime >= settimes[0])
                {
                    TutorialManagerScript.Instance.SetTimeCheckFlag(false); //Update内の時間を止める
                    TutorialManagerScript.Instance.SetPhaseCheck(false);
                    nowTime = 0.0f;
                    TutorialManagerScript.Instance.SetPhaseNumber(1);
                    Debug.Log("Phase1");
                }
                break;
            case 1:
                if (nowTime >= settimes[1])
                {
                    nowTime = 0.0f;
                    TutorialManagerScript.Instance.SetPhaseNumber(2);
                    Debug.Log("Phase2");
                }
                break;
            case 2:
                if (nowTime >= settimes[2])
                {
                    nowTime = 0.0f;
                    TutorialManagerScript.Instance.SetPhaseNumber(3);
                    Debug.Log("Phase3");
                }
                break;
            case 3:
                if (nowTime >= settimes[3])
                {
                    nowTime = 0.0f;
                    TutorialManagerScript.Instance.SetPhaseNumber(4);
                }
                break;
            case 4:
                if (nowTime >= settimes[4])
                {
                    nowTime = 0.0f;
                    TutorialManagerScript.Instance.SetPhaseNumber(5);
                }
                break;
            case 5:
                if (nowTime >= settimes[5])
                {
                    TutorialManagerScript.Instance.SetPhaseNumber(6);
                }
                break;
        }
    }
}

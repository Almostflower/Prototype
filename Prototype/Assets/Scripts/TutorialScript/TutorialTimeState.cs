using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTimeState : MonoBehaviour
{
    private float[] settimes = new float[5];    //説明画像を切り替える時間用の変数
    
    private float nowTime;//現在時刻


    // Start is called before the first frame update
    void Start()
    {
        nowTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        nowTime += Time.deltaTime;

        if(nowTime > settimes[0])
        {
            TutorialManagerScript.Instance.SetPhase(1);
        }
        else if(nowTime > settimes[1])
        {
            TutorialManagerScript.Instance.SetPhase(2);
        }
        else if(nowTime > settimes[2])
        {
            TutorialManagerScript.Instance.SetPhase(3);
        }
        else if(nowTime > settimes[3])
        {
            TutorialManagerScript.Instance.SetPhase(4);
        }
        else if(nowTime > settimes[4])
        {
            TutorialManagerScript.Instance.SetPhase(5);
        }
        else if(nowTime > settimes[5])
        {
            TutorialManagerScript.Instance.SetPhase(6);
        }
    }
}

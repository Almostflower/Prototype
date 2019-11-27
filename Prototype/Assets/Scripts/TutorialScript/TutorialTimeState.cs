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

    // Update is called once per frame
    void Update()
    {
        if(TutorialManagerScript.Instance.GetPhaseCheck() == true)
        {
            Debug.Log("true");
            nowTime += Time.deltaTime;
        }
        else
        {
            Debug.Log("false");
            nowTime = 0.0f;
        }

        if(nowTime >= settimes[0])
        {
            //TutorialManagerScript.Instance.SetPhaseCheck(false); //フェードアウトさせるときにこれを呼ぶ！
            nowTime = 0.0f;
            TutorialManagerScript.Instance.SetPhaseNumber(1);
            Debug.Log("Phase1");
        }
        else if(nowTime >= settimes[1])
        {
            nowTime = 0.0f;
            TutorialManagerScript.Instance.SetPhaseNumber(2);
            Debug.Log("Phase2");
        }
        else if(nowTime >= settimes[2])
        {
            nowTime = 0.0f;
            TutorialManagerScript.Instance.SetPhaseNumber(3);
            Debug.Log("Phase3");
        }
        else if(nowTime >= settimes[3])
        {
            nowTime = 0.0f;
            TutorialManagerScript.Instance.SetPhaseNumber(4);
        }
        else if(nowTime >= settimes[4])
        {
            nowTime = 0.0f;
            TutorialManagerScript.Instance.SetPhaseNumber(5);
        }
        else if(nowTime >= settimes[5])
        {
            TutorialManagerScript.Instance.SetPhaseNumber(6);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
public class PauseSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject TitleOn;

    [SerializeField]
    private GameObject TitleOff;

    [SerializeField]
    private GameObject RestartOn;

    [SerializeField]
    private GameObject RestartOff;

    [SerializeField]
    private GameObject ExitOn;

    [SerializeField]
    private GameObject ExitOff;

    [SerializeField]
    private GameObject Background;

    [SerializeField]
    private int SelectNum;

    private bool SelectFlag;

    //private bool TimeCountFlag;
    //private float FadeTime;

    // Start is called before the first frame update
    void Start()
    {
        //メモリー枯渇にならないように
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        SelectFlag = false;
        TitleOn.SetActive(false);
        TitleOff.SetActive(false);
        RestartOff.SetActive(false);
        RestartOn.SetActive(false);
        ExitOn.SetActive(false);
        ExitOff.SetActive(false);
        SelectNum = 0;
        Background.SetActive(false);
        //TimeCountFlag = false;
        //FadeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //if(TimeCountFlag)
        //{
        //    FadeTime += Time.deltaTime;
        //    
        //}
        //if(FadeTime >= 1.5f)
        //{
        //    //SceneChangeFunc();
        //}

        if(SceneStatusManager.Instance.PauseButton == -1)
        {
            SelectFlag = true;
        }
        else
        {
            SelectFlag = false;
            SelectOffFunc();
        }

        if(SelectFlag)
        {
            Background.SetActive(true);
            SelectFunc();
        }
        else
        {
            Background.SetActive(false);
        }

    }

    private void SceneChangeFunc()
    {
        //if(SelectNum == 0)
        //{
        //    SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        //}
        //if(SelectNum == 1)
        //{
        //    SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        //}
    }
    float selectresettime = 0.0f;
    float resettime = 0.0f;
    bool acadebuttonflag = false;
    bool selectflag = false;
    private void SelectFunc()
    {
        if(selectflag)
        {
            selectresettime += Time.deltaTime;

            if(selectresettime > 0.5f)
            {
                selectresettime = 0.0f;
                selectflag = false;
            }
        }
        if(acadebuttonflag)
        {
            resettime += Time.deltaTime;

            if(resettime > 0.5f)
            {
                resettime = 0.0f;
                acadebuttonflag = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !acadebuttonflag || Input.GetAxisRaw("Vertical") <= -1.0f && !acadebuttonflag)
        {
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
            acadebuttonflag = true;

            if (SelectNum < 3)
            {
                SelectNum++;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !acadebuttonflag || Input.GetAxisRaw("Vertical") >= 1.0f && !acadebuttonflag)
        {
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
            acadebuttonflag = true;
            if (SelectNum > 0)
            {
                SelectNum--;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (SelectNum < 3)
            {
                SelectNum++;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (SelectNum > 0)
            {
                SelectNum--;
            }
        }
        switch (SelectNum)
        {
            case 0:
                TitleOn.SetActive(true);
                TitleOff.SetActive(false);
                RestartOn.SetActive(false);
                RestartOff.SetActive(true);
                ExitOn.SetActive(false);
                ExitOff.SetActive(true);
                break;
            case 1:
                TitleOn.SetActive(false);
                TitleOff.SetActive(true);
                RestartOn.SetActive(true);
                RestartOff.SetActive(false);
                ExitOn.SetActive(false);
                ExitOff.SetActive(true);
                break;
            case 2:
                TitleOn.SetActive(false);
                TitleOff.SetActive(true);
                RestartOn.SetActive(false);
                RestartOff.SetActive(true);
                ExitOn.SetActive(true);
                ExitOff.SetActive(false);
                break;
            default:
                break;
        }

        if (SelectNum == 0)
        {
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button3) && !selectflag)
            {
                selectflag = true;
                SceneStatusManager.Instance.SetFadeIn(true);
                SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
                //TimeCountFlag = true;
            }
        }
        else if (SelectNum == 1)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.Joystick1Button3) && !selectflag)
            {
                selectflag = true;
                SceneStatusManager.Instance.PauseButton = 1;
                //SceneStatusManager.Instance.SetFadeIn(true);
                //TimeCountFlag = true;

            }
        }
        else if (SelectNum == 2)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.Joystick1Button3) && !selectflag)
            {
                selectflag = true;
                Quit();
            }
        }
    }
    void Quit()
    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#elif UNITY_STANDALONE
//        UnityEngine.Application.Quit();
//#endif
        Application.Quit();
    }
    private void SelectOffFunc()
    {
        TitleOn.SetActive(false);
        TitleOff.SetActive(false);
        RestartOn.SetActive(false);
        RestartOff.SetActive(false);
        ExitOn.SetActive(false);
        ExitOff.SetActive(false);
    }
}

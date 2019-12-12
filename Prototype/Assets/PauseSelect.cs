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
    private int SelectNum;

    private bool SelectFlag;

    private bool TimeCountFlag;
    private float FadeTime;

    // Start is called before the first frame update
    void Start()
    {
        SelectFlag = false;
        TitleOn.SetActive(false);
        TitleOff.SetActive(false);
        RestartOff.SetActive(false);
        RestartOn.SetActive(false);
        ExitOn.SetActive(false);
        ExitOff.SetActive(false);
        SelectNum = 0;
        TimeCountFlag = false;
        FadeTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeCountFlag)
        {
            FadeTime += Time.deltaTime;
        }
        if(FadeTime >= 1.5f)
        {
            SceneChangeFunc();
        }

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
            SelectFunc();
        }

    }

    private void SceneChangeFunc()
    {
        if(SelectNum == 0)
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
        if(SelectNum == 1)
        {
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        }
    }

    private void SelectFunc()
    {
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
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneStatusManager.Instance.SetFadeIn(true);
                TimeCountFlag = true;
            }
        }
        else if (SelectNum == 1)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneStatusManager.Instance.SetFadeIn(true);
                TimeCountFlag = true;
                //SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
            }
        }
        else if (SelectNum == 2)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Quit();
            }
        }
    }
    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
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

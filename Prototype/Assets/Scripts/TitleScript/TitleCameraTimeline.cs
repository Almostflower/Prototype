﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System;
public class TitleCameraTimeline : MonoBehaviour
{
    //private bool isMainColor = false;
    //[SerializeField] Color color1 = Color.white, color2 = Color.white;
    //[SerializeField] UnityEngine.UI.Image image = null;

    //[SerializeField]
    //CanvasGroup group = null;

    //[SerializeField]
    //Fade fade = null;

    private float AnimTimer;
    private bool AnimStartFlag,FadeFlag;
    // Start is called before the first frame update

    void Start()
    {
        FadeFlag = false;
        SceneStatusManager.Instance.SetFadeOut(true);

		//BGM再生
		SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.RTitle_BGM);
    }

    bool acadeselect = false;
    // Update is called once per frame
    void Update()
    {
        //キー入力した時にタイムラインでフェード＆演出が開始されていく
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button3) && !acadeselect)
        {
            acadeselect = true;
            //決定音
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Decision_SE);
            //PlayTimeline();
            AnimStartFlag = true;
        }

        if (AnimStartFlag && AnimTimer <= 10.0f)
        {
            AnimTimer += Time.deltaTime;
        }

        if(!FadeFlag)
        {
            if (AnimTimer >= 1.5f)
            {
                SceneStatusManager.Instance.SetFadeIn(true);
                FadeFlag = true;
            }
        }

        if (AnimTimer >= 10.0f)
        {
            AnimStartFlag = false;
        }

        if(SceneStatusManager.Instance.GetSceneChange())
        {
            if(SceneStatusManager.Instance.TitleStart == -1)
            {
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
                SceneStatusManager.Instance.SetSceneChange(false);
            }
            else
            {
                Quit();
                SceneStatusManager.Instance.SetSceneChange(false);
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
}

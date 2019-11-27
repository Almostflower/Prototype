﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;     //UIを使用可能にする
using UnityEngine;

public class TutorialState : MonoBehaviour
{
    [SerializeField]
    private int nowPhase;
    [SerializeField]
    private Image TutorialImage;
    float walfa = 0.0f;
    float speed = 0.01f;
    float red, green, blue;

    void Start()
    {
        red = TutorialImage.color.r;
        green = TutorialImage.color.g;
        blue = TutorialImage.color.b;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) //テスト入力
        {
            TutorialManagerScript.Instance.SetPhaseCheck(true);
        }
        if (nowPhase == TutorialManagerScript.Instance.GetPhaseNumber() && TutorialManagerScript.Instance.GetPhaseCheck() == true)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        TutorialImage.color = new Color(red, green, blue, walfa);
        if(walfa <= 1f)
        {
            walfa += speed;
        }
    }

    void FadeOut()
    {
        TutorialImage.color = new Color(red, green, blue, walfa);
        if(walfa >= 0f)
        {
            walfa -= speed;
        }
    }
}

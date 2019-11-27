using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;     //UIを使用可能にする
using UnityEngine;

public class TutorialState : MonoBehaviour
{
    [SerializeField]
    private int nowPhase;

    float alfa;
    float speed = 0.01f;
    float red, green, blue;

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
    }

    void Update()
    {
        if(nowPhase == TutorialManagerScript.Instance.GetPhase())
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
        GetComponent<Image>().color = new Color(red, green, blue, alfa);
        alfa += speed;
    }

    void FadeOut()
    {
        GetComponent<Image>().color = new Color(red, green, blue, alfa);
        if(alfa >= 0.0f)
        {
            alfa -= speed;
        }
    }
}

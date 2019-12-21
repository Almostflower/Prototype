using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImgGlowIntensiveChenger : MonoBehaviour
{

    [SerializeField]
    bool type = false;

    [SerializeField]
    GlowImage img;
    [SerializeField]
    float defaultvalue = 0.0f;
    [SerializeField]
    float LimitValue = 0.0f;
    [SerializeField]
    float addvalue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!type)
        {
            UIGlowIntensiveMinus(img);
        }
        else
        {
            UIGlowIntensiveAdd(img);
        }
    }

    public void UIGlowIntensiveMinus(GlowImage img)
    {
        if (img.glowIntensitive > LimitValue)
        {
            img.glowIntensitive -= addvalue;
        }
        else
        {
            img.glowIntensitive = defaultvalue;
        }
    }

    public void UIGlowIntensiveAdd(GlowImage img)
    {
        if (img.glowIntensitive < LimitValue)
        {
            img.glowIntensitive += addvalue;
        }
        else
        {
            img.glowIntensitive = defaultvalue;
        }
    }
}

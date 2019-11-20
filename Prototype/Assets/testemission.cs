using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testemission : MonoBehaviour
{
    private Renderer _renderer;
    private float colortime;
    private float changecolor, changecolor1, changecolor2;
    // Use this for initialization
    void Start()
    {
        changecolor = 0.0f;
        colortime = 0.0f;
        _renderer = GetComponent<Renderer>();
    }
    int type = 0;
    // Update is called once per frame
    void Update()
    {
        colortime += Time.deltaTime;
        var originalMaterial = new Material(_renderer.material);

        _renderer.material.EnableKeyword("_EMISSION"); //キーワードの有効化を忘れずに
        
        if(colortime > 2.0f)
        {
            if(type < 3)
            type += 1;

            colortime = 0.0f;
        }
        switch(type)
        {
            case 0:
                if (changecolor <= 1)
                {
                    _renderer.material.SetColor("_EmissionColor", new Color(changecolor, 0, 0)); //赤色に光らせる
                    changecolor += 0.1f;
                }
                break;
            case 1:
                if (changecolor1 <= 1)
                {
                    _renderer.material.SetColor("_EmissionColor", new Color(0, changecolor1, 0)); //赤色に光らせる
                    changecolor1 += 0.1f;
                }
                break;
            case 2:
                if (changecolor2 <= 1)
                {
                    _renderer.material.SetColor("_EmissionColor", new Color(0, 0, changecolor2)); //赤色に光らせる
                    changecolor2 += 0.1f;
                }
                break;
        }
    }
}

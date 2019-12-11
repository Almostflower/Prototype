using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class ResultOperation : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private float Wmargin = 0.0f;
    //private float Hmargin = 1.0f;
    private void Awake()
    {
        cam.rect = new Rect(0.0f, 0.0f, Wmargin, 1.0f);
    }
    private void Update()
    {
        if(Wmargin < 1.0f)
        {
            Wmargin += 0.005f;
        }
        //if(Hmargin > 0.0f)
        //{
        //    Hmargin -= 0.01f;
        //}
        cam.rect = new Rect(0.0f, 0.0f, Wmargin, 1.0f);
    }
}

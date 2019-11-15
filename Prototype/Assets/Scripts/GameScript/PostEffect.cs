using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    public Material outline;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // ポストエフェクトをかける
        Graphics.Blit(source, destination, outline);
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    
    //}
    //
    //// Update is called once per frame
    //public override void UpdateNormal()
    //{
    //    
    //}
}

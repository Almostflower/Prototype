using System.Collections;
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

    [SerializeField]
    private PlayableDirector playableDirector;
    private float AnimTimer;
    private bool AnimStartFlag,FadeFlag;
    // Start is called before the first frame update
    void Start()
    {
        FadeFlag = false;
        SceneStatusManager.Instance.SetFadeOut(true);
    }

    // Update is called once per frame
    void Update()
    {
        //キー入力した時にタイムラインでフェード＆演出が開始されていく
        if(Input.GetKeyDown(KeyCode.Return))
        {
            PlayTimeline();
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
            StopTimeline();
            AnimStartFlag = false;
        }

        if(SceneStatusManager.Instance.GetSceneChange())
        {
            SceneManager.LoadSceneAsync(1,LoadSceneMode.Single);
        }
    }

    void PlayTimeline()
    {
        playableDirector.Play();
    }

    void StopTimeline()
    {
        playableDirector.Stop();
    }
}

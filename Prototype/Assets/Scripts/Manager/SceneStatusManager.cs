using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStatusManager : SingletonMonoBehaviour<SceneStatusManager>
{
    private bool SceneFadeIn;
    private bool SceneFadeOut;
    private bool GameFade;
    private bool SceneChange;
    private void Awake()
    {
        SceneFadeIn = false;
        SceneFadeOut = false;
        GameFade = false;
        SceneChange = false;
    }
    // Start is called before the first frame update
    void Start()
    {
 
    }

    public bool GetFadeIn()
    {
        return SceneFadeIn;
    }

    public bool GetFadeOut()
    {
        return SceneFadeOut;
    }

    public bool GetGameFade()
    {
        return GameFade;
    }

    public bool GetSceneChange()
    {
        return SceneChange;
    }

    public void SetFadeIn(bool fadein)
    {
        SceneFadeIn = fadein;
    }

    public void SetFadeOut(bool fadeout)
    {
        SceneFadeOut = fadeout;
    }

    public void SetGameFade(bool gamefade)
    {
        GameFade = gamefade;
    }

    public void SetSceneChange(bool scenechange)
    {
        SceneChange = scenechange;
    }
}

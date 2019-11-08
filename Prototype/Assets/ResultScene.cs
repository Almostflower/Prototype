﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResultScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneStatusManager.Instance.SetFadeOut(true);
        SceneStatusManager.Instance.SetFadeIn(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneStatusManager.Instance.SetFadeIn(false);
            SceneStatusManager.Instance.SetFadeIn(true);
        }

        if (SceneStatusManager.Instance.GetSceneChange())
        {
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}

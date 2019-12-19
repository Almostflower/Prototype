using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResultScene : MonoBehaviour
{
    private bool SelectFlag;
    // Start is called before the first frame update
    void Start()
    {
        SelectFlag = false;
        SceneStatusManager.Instance.SetFadeOut(true);
        SceneStatusManager.Instance.SetFadeIn(false);

		//BGM再生
		SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.RRResult_BGM);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button3) && !SelectFlag)
        {
            SelectFlag = true;
            //決定音
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Decision_SE);

			SceneStatusManager.Instance.SetFadeIn(false);
            SceneStatusManager.Instance.SetFadeIn(true);
        }

        if (SceneStatusManager.Instance.GetSceneChange())
        {
			SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            SceneStatusManager.Instance.SetSceneChange(false);
        }
    }
}

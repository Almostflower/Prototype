using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialScene : MonoBehaviour
{
    //[SerializeField] UnityEngine.UI.Image image = null;
    
    // Start is called before the first frame update
    void Start()
    {
        SceneStatusManager.Instance.SetFadeOut(true);
        SceneStatusManager.Instance.SetFadeIn(false);

		//BGM再生
		SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.Tutorial_BGM);
	}

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
			//決定音
			SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Decision_SE);

			//SceneStatusManager.Instance.SetFadeIn(false);
            //SceneStatusManager.Instance.SetFadeIn(true);
        }

        if(SceneStatusManager.Instance.GetSceneChange())
        {
			SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        }
    }
}

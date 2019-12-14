using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class OriginalFade : MonoBehaviour
{
    private bool isMainColor = false;
    [SerializeField] Color color1 = Color.white, color2 = Color.white;
    [SerializeField] UnityEngine.UI.Image image = null;

    [SerializeField]
    CanvasGroup group = null;

    [SerializeField]
    Fade fade = null;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (SceneStatusManager.Instance.GetFadeIn())
        {
            Debug.Log("FadeNow");
            Fadein();
            SceneStatusManager.Instance.SetFadeIn(false);
        }

        if(SceneStatusManager.Instance.GetFadeOut())
        {
            Debug.Log("FadeOut");
            SceneStatusManager.Instance.SetFadeOut(false);
            Fadeout();
        }
        if(SceneStatusManager.Instance.GetGameFade())
        {
            SceneStatusManager.Instance.SetGameFade(false);
            GameFade();
        }

        //if(Input.GetKeyDown(KeyCode.I))
        //{
        //    Fadein();
        //}
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    Fadeout();
        //}
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void Fadein()
    {
        group.blocksRaycasts = false;
        fade.FadeIn(3.0f, () =>
        {
            //image.color = (isMainColor) ? color1 : color2;
            //isMainColor = !isMainColor;
            image.color = new Color(0,0,0, 255);
            fade.FadeOut(0, () => {
                group.blocksRaycasts = true;
                SceneStatusManager.Instance.SetSceneChange(true);
            });
        });

    }
    /// <summary>
    /// フェードアウト
    /// </summary>
    public void Fadeout()
    {
        group.blocksRaycasts = false;


        fade.FadeIn(0, () =>
        {
            image.color = new Color(255, 255, 255, 0);
            //image.color = (isMainColor) ? color1 : color2;
            //isMainColor = !isMainColor;
            fade.FadeOut(3.0f, () => {
                group.blocksRaycasts = true;
            });
        });
    }

    /// <summary>
    /// ゲームプレイ中にフェードかけたいときにフェードイン＆アウトを両方させたいとき
    /// </summary>
    public void GameFade()
    {
        group.blocksRaycasts = false;

        fade.FadeIn(1, () =>
        {
            image.color = (isMainColor) ? color1 : color2;
            isMainColor = !isMainColor;
            fade.FadeOut(1, () => {
                group.blocksRaycasts = true;
            });
        });
    }
}

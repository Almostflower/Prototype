using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Loading : MonoBehaviour
{

    //　非同期動作で使用するAsyncOperation
    private AsyncOperation async;
    //　シーンロード中に表示するUI画面
    [SerializeField]
    private GameObject loadUI;
    //　読み込み率を表示するスライダー
    [SerializeField]
    private Slider slider;

	/// <summary>
	/// ローディングバーのイメージ
	/// </summary>
	[SerializeField]
	private List<Image> lodingbar_ = new List<Image>();

	/// <summary>
	/// 入れ替えの長さ
	/// </summary>
	[SerializeField]
	private float timing_time_ = 0;

	/// <summary>
	/// カウント
	/// </summary>
	private float count = 0;

	void Start()
	{
		foreach (var i in lodingbar_.Select((value, index) => new { value, index }))
		{
			lodingbar_[i.index].enabled = false;
		}
	}

	public void NextScene()
    {
        //　ロード画面UIをアクティブにする
        loadUI.SetActive(true);

        //　コルーチンを開始
        StartCoroutine("LoadData");
    }

    IEnumerator LoadData()
    {
        // シーンの読み込みをする
        async = SceneManager.LoadSceneAsync("Prototype");

        //　読み込みが終わるまで進捗状況をスライダーの値に反映させる
        while (!async.isDone)
        {
			count += Time.deltaTime;

			if (count > timing_time_)
			{
				count = 0;

				if (lodingbar_[lodingbar_.Count() - 1].enabled)
				{
					foreach (var i in lodingbar_.Select((value, index) => new { value, index }))
					{
						lodingbar_[i.index].enabled = false;
					}
				}
				else
				{
					foreach (var i in lodingbar_.Select((value, index) => new { value, index }))
					{
						if (!lodingbar_[i.index].enabled)
						{
							lodingbar_[i.index].enabled = true;
							break;
						}
					}
				}
			}

			var progressVal = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progressVal;
            yield return null;
        }
    }
}

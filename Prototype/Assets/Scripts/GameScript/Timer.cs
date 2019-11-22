using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : BaseMonoBehaviour
{
	/// <summary>
	/// 制限時間のフラグ
	/// </summary>
	private bool limit_time_ = false;

	/// <summary>
	/// 制限時間の設定
	/// </summary>
	[SerializeField]
	private float seconds_;

	/// <summary>
	/// UIの表示
	/// </summary>
	[SerializeField]
	private ImageNo image_;

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
	}

	void Start()
	{
		limit_time_ = false;
	}

	public override void UpdateNormal()
	{
		// 制限時間が0秒以下なら何もしない
		if (limit_time_)
		{
			return;
		}

		// いったんトータルの制限時間を計測
		seconds_ -= Time.deltaTime;

		// タイマー表示用UIに時間を表示する
		image_.SetNo((int)seconds_);

        // 制限時間いかになったらコンソールに「制限時間終了」という文字列を表示する
        if (seconds_ <= 0f)
		{
			// タイムオーバーSE
			SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.TimeOver_SE);

			SceneStatusManager.Instance.SetFadeIn(false);
			SceneStatusManager.Instance.SetFadeIn(true);

			// 次のシーンへ
			if (SceneStatusManager.Instance.GetSceneChange())
			{
				SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
			}
			seconds_ = 0.00f;
			limit_time_ = true;
		}
	}
}
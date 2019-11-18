using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : BaseMonoBehaviour
{
	/// <summary>
	/// デバッグモード
	/// </summary>
	//[SerializeField]
	//bool debug_mode_ = true;

	/// <summary>
	/// ゲージスクリプト
	/// </summary>
	//[SerializeField]
	//private Gauge gauge_;

	/// <summary>
	/// 制限時間のフラグ
	/// </summary>
	private bool limit_time_ = false;

	/// <summary>
	/// Ui表示
	/// </summary>
	[SerializeField]
	private Text timer_text_;

	/// <summary>
	/// 制限時間の設定
	/// </summary>
	[SerializeField]
	private float seconds_;
	public float Seconds
	{
		get { return seconds_; }
	}

	/// <summary>
	/// 一回きりのフラグ
	/// </summary>
	private bool once_ = false;

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
		once_ = false;
	}

	public override void UpdateNormal()
	{
		if(!once_)
		{
			//gauge_.SetMaxValue(seconds_);
			once_ = true;
		}

		// 制限時間が0秒以下なら何もしない
		if (limit_time_)
		{
			return;
		}

		// いったんトータルの制限時間を計測
		seconds_ -= Time.deltaTime;
        //gauge_.TimerValue = seconds_;

        //// デバッグモードだったら表示
        //if (debug_mode_)
        //{
        timer_text_.enabled = true;
        // タイマー表示用UIテキストに時間を表示する
        timer_text_.text = seconds_.ToString("F2");
        //}
        //else
        //{
        //	timer_text_.enabled = false;
        //}

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
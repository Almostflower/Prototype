using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	/// <summary>
	/// デバッグモード
	/// </summary>
	[SerializeField]
	bool debug_mode_ = true;

	/// <summary>
	/// トータル制限時間
	/// </summary>
	[System.NonSerialized]
	public float total_time_;
	/// <summary>
	/// テキストオブジェクト
	/// </summary> 
	[SerializeField]
	private GameObject text_;
	/// <summary>
	/// Ui表示
	/// </summary>
	[SerializeField]
	private Text timerText;
	/// <summary>
	/// 制限時間の設定（分） 
	/// </summary>
	public int minute_;
	/// <summary>
	/// 制限時間の設定（秒）
	/// </summary>
	public float seconds_;
	/// <summary>
	/// 昔の秒数
	/// </summary>
	private float old_seconds_;

	void Start()
	{
		total_time_ = minute_ * 60 + seconds_;
		old_seconds_ = 0f;
		//timerText = GetComponentInChildren<Text>();
	}

	void Update()
	{
		// 制限時間が0秒以下なら何もしない
		if(total_time_ <= 0f)
		{
			return;
		}

		// いったんトータルの制限時間を計測
		total_time_ = minute_ * 60 + seconds_;
		total_time_ -= Time.deltaTime;

		// 再設定
		minute_ = (int)total_time_ / 60;
		seconds_ = total_time_ - minute_ * 60;

		// デバッグモードだったら表示
		if (debug_mode_)
		{
			text_.SetActive(true);
			// タイマー表示用UIテキストに時間を表示する
			if ((int)seconds_ != (int)old_seconds_)
			{
				timerText.text = minute_.ToString("00") + ":" + ((int)seconds_).ToString("00");
			}
		}
		else
		{
			text_.SetActive(false);
		}
		old_seconds_ = seconds_;

		// 制限時間いかになったらコンソールに「制限時間終了」という文字列を表示する
		if(total_time_ <= 0f)
		{
			Debug.Log("制限時間終了");
		}
	}
}
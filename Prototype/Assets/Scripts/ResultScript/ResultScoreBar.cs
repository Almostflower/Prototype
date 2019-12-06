﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ResultScoreBar : MonoBehaviour
{
	/// <summary>
	/// デバッグモード
	/// </summary>
	[SerializeField]
	bool debug_mode_ = true;

	/// <summary>
	/// ゲージスクリプト
	/// </summary>
	[SerializeField]
	private Gauge gauge_;

	/// <summary>
	/// Ui表示
	/// </summary>
	[SerializeField]
	private Text timer_text_;

	/// <summary>
	/// 合計スコア
	/// </summary>
	private float total_score_ = 0;

	/// <summary>
	/// 最大スコア
	/// </summary>
	private float max_score_ = 0;

	/// <summary>
	/// 一回きり
	/// </summary>
	private bool once_ = false;

	/// <summary>
	/// 一番いいスコア
	/// </summary>
	[SerializeField]
	private float amazing_ = 0.7f;

	/// <summary>
	/// 普通のスコア
	/// </summary>
	[SerializeField]
	private float good_ = 0.5f;

	/// <summary>
	/// 悪いスコア
	/// </summary>
	[SerializeField]
	private float error_ = 0.3f;

	[SerializeField]
	private List<Image> operation_ = new List<Image>();

	private enum OPERATION
	{
		Amazing,
		Good,
		Error
	};

	void Start()
    {
		max_score_ = Score.GetMaxScore();
		total_score_ = Score.GetTotalScore();
		gauge_.GaugeValue = total_score_;

		foreach(var i in operation_.Select((value, index) => new { value, index }))
		{
			operation_[i.index].enabled = false;
		}
	}

    void Update()
    {
		if(!once_)
		{
			once_ = true;
			gauge_.SetMaxValue(max_score_);
		}

		if (total_score_ >= max_score_ * amazing_)
		{
			operation_[(int)OPERATION.Amazing].enabled = true;
		}
		else if(total_score_ >= max_score_ * good_ && total_score_ < max_score_ * amazing_)
		{
			operation_[(int)OPERATION.Good].enabled = true;
		}
		else if(total_score_ < max_score_ * error_)
		{
			operation_[(int)OPERATION.Error].enabled = true;
		}

		// デバッグモードだったら表示
		if (debug_mode_)
		{
			timer_text_.enabled = true;
			// タイマー表示用UIテキストに時間を表示する
			timer_text_.text = total_score_.ToString("000");
		}
		else
		{
			timer_text_.enabled = false;
		}
	}
}

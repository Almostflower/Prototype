using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : BaseMonoBehaviour
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
	/// 一回きりのフラグ
	/// </summary>
	private bool once_ = false;

	/// <summary>
	/// スコア合計
	/// </summary>
	private int total_score_ = 0;
	public int TotalScore
	{
		get { return total_score_; }
	}

	/// <summary>
	/// スコア最大値
	/// </summary>
	[SerializeField]
	private int maxscore_ = 100;
	public int MaxScore
	{
		get { return maxscore_; }
	}

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
	}

	void Start()
    {
		once_ = false;
		total_score_ = maxscore_ / 2;
    }

	public override void UpdateNormal()
	{
		if (!once_)
		{
			gauge_.SetMaxValue(maxscore_);
			once_ = true;
		}

		gauge_.GaugeValue = total_score_;

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

		// BGM処理
		if (total_score_ > maxscore_ * 0.7f)
		{
			SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.StageLight_BGM);
		}
		else if(total_score_ > maxscore_ * 0.3f && total_score_ < maxscore_ * 0.7f - 1)
		{
			SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.StageSelect_BGM);
		}
		else if(total_score_ < maxscore_ * 0.3f - 1)
		{
			SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.StageDark_BGM);
		}
    }

	/// <summary>
	/// スコアの値をもらう
	/// </summary>
	/// <param name="score"></param>
	public void SetScore(int score)
	{
		total_score_ += score;
	}
}

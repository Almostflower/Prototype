using System.Collections;
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
	private ResultGauge gauge_;

	/// <summary>
	/// Ui表示
	/// </summary>
	[SerializeField]
	private Text timer_text_;

	/// <summary>
	/// 合計スコア
	/// </summary>
	private int total_score_ = 0;

	/// <summary>
	/// 最大スコア
	/// </summary>
	private int max_score_ = 0;

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

	/// <summary>
	/// 画像
	/// </summary>
	[SerializeField]
	private List<Image> operation_ = new List<Image>();

    [SerializeField]
    private ResultPlayer playerinfo;
    /// <summary>
    /// スコアの上がるスピード
    /// </summary>
    [SerializeField]
	private float count_speed_ = 0.005f;

	private bool once_ = false;

	private bool countstopSE_flag_ = false;

	private bool scoreSE_flag_ = false;

	[SerializeField]
	private Count count_script_;

	private bool scorecountupE_flag_ = false;

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
		gauge_.GaugeValue = 0;
		once_ = false;
		countstopSE_flag_ = false;
		scoreSE_flag_ = false;
		scorecountupE_flag_ = false;

		foreach (var i in operation_.Select((value, index) => new { value, index }))
		{
			operation_[i.index].enabled = false;
		}
	}

    void Update()
    {
		if(!once_)
		{
			gauge_.SetMaxValue(max_score_);
			once_ = true;
		}

		// 数字のカウントの最後が終わるまで下の処理はしない
		if (!count_script_.LastCountSE)
		{
			return;
		}

		// スコアカウントアップ開始
		if (!scorecountupE_flag_)
		{
			// スコアカウントアップ
			SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.ScoreCountUp_SE);
			scorecountupE_flag_ = true;
		}

		// カウントアップ
		gauge_.GaugeValue += count_speed_;

		// カウントストップ
		if (gauge_.GaugeValue > total_score_)
		{
			if (!countstopSE_flag_)
			{
				countstopSE_flag_ = true;
			}
			gauge_.GaugeValue = total_score_;
		}

		// カウントダウン終わるまで下の処理はしない
		if (!countstopSE_flag_)
		{
			return;
		}

		if (!scoreSE_flag_)
		{
			// SEストップ
			SoundManager.SingletonInstance.AllStopSE();
			// リザルトで文字表示させる
			SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.ScoreDecision_SE);
			scoreSE_flag_ = true;
		}

		// 点数に応じて文字を変更
		if (total_score_ >= max_score_ * amazing_)
		{
			operation_[(int)OPERATION.Amazing].enabled = true;
            playerinfo.JudgeType = 2;
        }
		else if(total_score_ >= max_score_ * good_ && total_score_ < max_score_ * amazing_)
		{
			operation_[(int)OPERATION.Good].enabled = true;
            playerinfo.JudgeType = 1;
        }
		else if(total_score_ < max_score_ * error_)
		{
            operation_[(int)OPERATION.Error].enabled = true;
            playerinfo.JudgeType = 0;
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

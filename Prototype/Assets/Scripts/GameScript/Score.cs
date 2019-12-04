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
	[System.NonSerialized]
	public static int total_score_ = 0;
	public static int GetTotalScore()
	{
		return total_score_;
	}

	/// <summary>
	/// スコア最大値
	/// </summary>
	[SerializeField]
	private int maxscore_ = 100;
	public static int maxscore = 100;
	public int MaxScore
	{
		get { return maxscore_; }
	}
	public static int GetMaxScore()
	{
		return maxscore;
	}

	/// <summary>
	/// 良いギフトの管理
	/// </summary>
	[System.NonSerialized]
	public static int gift_good_ = 0;
	public static int GetGiftGood()
	{
		return gift_good_;
	}

	/// <summary>
	/// 悪いギフトの管理
	/// </summary>
	[System.NonSerialized]
	public static int gift_bad_ = 0;
	public static int GetGiftBad()
	{
		return gift_bad_;
	}

	/// <summary>
	/// 良いウサギの寝床
	/// </summary>
	[System.NonSerialized]
	public static int rabbit_good_ = 0;
	public static int GetRabbitGood()
	{
		 return rabbit_good_; 
	}

	/// <summary>
	/// 悪いウサギの寝床
	/// </summary>
	[System.NonSerialized]
	public static int rabbit_bad_ = 0;
	public static int GetRabbitBad()
	{
		return rabbit_bad_;
	}

	public enum GIFTSTATUS
	{
		nongift,
		giftgood,
		giftbad,
		max
	};

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
		gift_good_ = 0;
		gift_bad_ = 0;
		rabbit_good_ = 0;
		rabbit_bad_ = 0;
		maxscore = maxscore_;
	}

	public override void UpdateNormal()
	{
		if (!once_)
		{
			gauge_.SetMaxValue(maxscore_);
			once_ = true;
		}

		if (total_score_ < 0)
		{
			total_score_ = 0;
		}
		else if(total_score_ > maxscore_)
		{
			total_score_ = maxscore_;
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
            GameStatusManager.Instance.SetGameState(GameStatusManager.GameState.GOOD);

        }
		else if(total_score_ > maxscore_ * 0.3f && total_score_ < maxscore_ * 0.7f - 1)
		{
			SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.StageSelect_BGM);
            GameStatusManager.Instance.SetGameState(GameStatusManager.GameState.NORMAL);
        }
		else if(total_score_ < maxscore_ * 0.3f - 1)
		{
			SoundManager.SingletonInstance.PlayBGM(SoundManager.BGMLabel.StageDark_BGM);
            GameStatusManager.Instance.SetGameState(GameStatusManager.GameState.BAD);
        }
    }

	/// <summary>
	/// スコアの値をもらう
	/// </summary>
	/// <param name="score"></param>
	public void SetScore(int score, int giftstatus = (int)GIFTSTATUS.nongift)
	{
		total_score_ += score;

		switch (giftstatus)
		{
			case (int)GIFTSTATUS.giftgood:
				gift_good_++;
				break;
			case (int)GIFTSTATUS.giftbad:
				gift_bad_++;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 良いウサギのセット関数
	/// </summary>
	public void SetRabbitGood()
	{
		rabbit_good_++;
	}

	/// <summary>
	/// 悪いウサギのセット関数
	/// </summary>
	public void SetRabbitBad()
	{
		rabbit_bad_++;
	}
}

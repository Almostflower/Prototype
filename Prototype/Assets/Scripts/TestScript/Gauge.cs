using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : BaseMonoBehaviour
{
	///////////////////////////////////////////////////////////////////
	/// プロトを見て変える
	/// <summary>
	/// 1本のゲージで見せるモード
	/// </summary>
	private bool debug_one_time_ = false;
	private float dustlimittime_ = 0;
	///////////////////////////////////////////////////////////////////


	/// <summary>
	/// スライダー
	/// </summary>
	[SerializeField]
	private Slider slider_;

	/// <summary>
	/// スライダーの中の背景画像
	/// </summary>
	[SerializeField]
	private Image background_;

	/// <summary>
	/// スライダーの中の動かす背景
	/// </summary>
	[SerializeField]
	private Image fill_;

	/// <summary>
	/// メインカメラのオブジェクト
	/// </summary>
	private Camera main_camera_;

	/// <summary>
	/// 減算する値
	/// </summary>
	private float timer_value_;
	public float TimerValue
	{
		get { return timer_value_; }
		set { timer_value_ = value; }
	}

	/// <summary>
	/// 一回きりのフラグ
	/// </summary>
	private bool once_ = false;

	/// <summary>
	/// キャンバスのオブジェクト
	/// </summary>
	[SerializeField]
	private GameObject canvas_;

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
	}

	void Start()
	{
		////////////////////////////////////////////////////////
		/// 後でメインSceneのカメラに変更する
		// メインカメラを取得
		main_camera_ = Camera.main;

		once_ = false;

		// 最大秒数をスライダーのマックス値に代入
		slider_.maxValue = timer_value_;
	}

	public override void UpdateNormal()
	{
		if (debug_one_time_)
		{
			background_.color = new Color32(0, 0, 0, 255);
			if (timer_value_ < dustlimittime_)
			{
				fill_.color = new Color32(255, 255, 0, 255);
			}
			else
			{
				fill_.color = new Color32(0, 255, 0, 255);
			}
		}

		// HPゲージに値を設定
		slider_.value = timer_value_;

		// 常にカメラに向く
		canvas_.transform.LookAt(-main_camera_.transform.position);

		// スライダーの値が0以下になったら最大値に戻す
		if (slider_.value <= 0 && once_)
		{
			slider_.value = slider_.maxValue;
			once_ = false;
		}
	}

	/// <summary>
	/// ギフトの値をスライダーマックス値にセット関数
	/// </summary>
	/// /// <param name="seconds"></param>
	public void SetGiftValue(float seconds)
	{
		slider_.maxValue = seconds;
		fill_.color = new Color32(255, 255, 0, 255);
		slider_.value = seconds;
		background_.color = new Color32(0, 0, 0, 255);
	}

	/// <summary>
	/// 最初のスライダーマックス値のセット関数
	/// </summary>
	/// <param name="val"></param>
	/// <param name="dusttime"></param>
	public void SetMaxValue(float val, float dusttime = 0, bool doubletime = false, bool giftonce = false)
	{
		slider_.maxValue = val;
		slider_.value = slider_.maxValue;
		once_ = giftonce;

		///////////////////////////////////////////////////
		/// ここもプロトを見て変える
		dustlimittime_ = dusttime;
		debug_one_time_ = doubletime;
	}
}

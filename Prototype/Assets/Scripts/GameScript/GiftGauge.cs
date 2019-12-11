using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftGauge : BaseMonoBehaviour
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
	/// 赤い画像
	/// </summary>
	[SerializeField]
	private Image dust_time_;

	/// <summary>
	/// 緑の画像
	/// </summary>
	[SerializeField]
	private Image bad_time_;

	/// <summary>
	/// キャンバスのオブジェクト
	/// </summary>
	[SerializeField]
	private GameObject canvas_;

	/// <summary>
	/// 減算する値
	/// </summary>
	private float gauge_value_;
	public float GaugeValue
	{
		set { gauge_value_ = value; }
	}

	/// <summary>
	/// 最大値
	/// </summary>
	private float gauge_max_value_;

	/// <summary>
	/// 悪い状態のフラグ
	/// </summary>
	private bool badflag = false;

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	protected override void Awake()
	{
		base.Awake();
	}


	void Start()
    {
		badflag = false;
		this.transform.localPosition = new Vector3(0.0f, this.transform.position.y, 0.0f);
		this.transform.Rotate(new Vector3(0, 1, 0), 180);
	}

    // Update is called once per frame
    private void Update()
    {
		///////////////////////////////////////////////////////////////////
		/// プロトを見て変える
		/// <summary>
		if (debug_one_time_)
		{
			dust_time_.fillAmount = gauge_value_ / gauge_max_value_;

			if(gauge_value_ < dustlimittime_)
			{
				bad_time_.enabled = false;
			}
			else
			{
				bad_time_.fillAmount = gauge_value_ / gauge_max_value_;
			}
		}
		else
		{
			if (bad_time_.fillAmount <= 0 && gauge_value_ > 0)
			{
				// ごみになる状態になるまでの処理
				dust_time_.fillAmount = gauge_value_ / gauge_max_value_;
				badflag = true;
			}

			if (!badflag)
			{
				// 悪い状態になるまでの処理
				bad_time_.fillAmount = gauge_value_ / gauge_max_value_;
			}
		}

		Vector3 p = Camera.main.transform.position;
		p.y = transform.position.y;
		// 常にカメラに向く
		canvas_.transform.LookAt(p);
	}

	public void SetMaxValue(float gaugemaxval, float dusttime = 0, bool doubletime = false)
	{
		gauge_max_value_ = gaugemaxval;

		///////////////////////////////////////////////////
		/// ここもプロトを見て変える
		dustlimittime_ = dusttime;
		debug_one_time_ = doubletime;
	}
}

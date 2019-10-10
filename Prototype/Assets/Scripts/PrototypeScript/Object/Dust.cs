using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
	/// <summary>
	/// 全部の箱が開いて時間で開いたり閉じたりするモード
	/// </summary>
	// 使用できるか否か
	bool dust_ = false;

	// 固定時間
	float limit_time_ = 0;

	// 開いている時間
	float open_time_ = 0;

	// 閉じている時間
	float close_time_ = 0;

	/// <summary>
	/// 一つが閉じたら他が開くモード
	/// </summary>
	// 固定時間
	float limit_oneopne_time = 0;

	// 開いている時間
	float oneopen_time_ = 0;

	/// <summary>
	/// デバッグモード
	/// </summary>
	// 全部の箱が開いて時間で開いたり閉じたりするモード
	bool all_opneclose_ = true;

	// 一つが閉じたら他が開くモード
	bool one_opneclose_ = false;

    void Start()
    {
    }

	private void Awake()
	{
	}

	private void Update()
    {
		// 	全部の箱が開いて時間で開いたり閉じたりするモード
		if(all_opneclose_)
		{
			AllOpneClose();
			one_opneclose_ = false;
		}

		// 一つが閉じたら他が開くモード
		if (one_opneclose_)
		{
			OneOpneClose();
			all_opneclose_ = false;
		}
	}

	/// <summary>
	/// 全部の箱が開いて時間で開いたり閉じたりするモード
	/// </summary>
	private void AllOpneClose()
	{
		if (dust_)
		{
			//Debug.Log("ごみを入れられるよ");
			GetComponent<Renderer>().material.color = Color.blue;
			// ごみを入れる処理
			//
			//

			// 閉じるまでのカウントダウン
			this.open_time_ -= Time.deltaTime;

			// 開いてる時間が終わったら
			if (this.open_time_ < 0)
			{
				this.dust_ = false;
				this.open_time_ = this.limit_time_;
			}
		}
		else
		{
			//Debug.Log("閉じているよ");
			GetComponent<Renderer>().material.color = Color.red;

			// 開くまでのカウントダウン
			this.close_time_ -= Time.deltaTime;

			// 閉じてる時間が終わったら
			if (this.close_time_ < 0)
			{
				this.dust_ = true;
				this.close_time_ = this.limit_time_;
			}
		}
	}

	/// <summary>
	/// 一つが閉じたら他が開くモード
	/// </summary>
	private void OneOpneClose()
	{
		if(this.dust_)
		{
			GetComponent<Renderer>().material.color = Color.blue;
			// ごみを入れる処理
			//
			//

			// 閉じるまでのカウントダウン
			this.oneopen_time_ -= Time.deltaTime;
			
			if(this.oneopen_time_ < 0)
			{
				GetComponent<Renderer>().material.color = Color.red;
				this.dust_ = false;
				this.oneopen_time_ = this.limit_oneopne_time;
			}
		}
	}

	/// <summary>
	/// ごみ箱空いてるかのセットゲット
	/// </summary>
	public bool SetOpenDust
	{
		set { this.dust_ = value; }
		get { return this.dust_; }
	}

	/// <summary>
	/// それぞれの開く時間のセッター
	/// </summary>
	/// <param name = "time">1</param>
	public void SetLimitTime(float time)
	{
		this.limit_time_ = time;
		this.open_time_ = this.limit_time_;
		this.close_time_ = this.limit_time_;
	}

	/// <summary>
	/// 開く時間のセッター
	/// </summary>
	/// <param name = "time">2</param>
	public void SetOneOpenTime(float time)
	{
		this.limit_oneopne_time = time;
		this.oneopen_time_ = this.limit_oneopne_time;
	}

	/// <summary>
	/// 全部の箱が開いて時間で開いたり閉じたりするモードセッター
	/// </summary>
	/// <param name = "opne">3</param>
	public void SetAllOpneClose(bool opne)
	{
		all_opneclose_ = opne;
	}

	/// <summary>
	/// 一つが閉じたら他が開くモードセッター
	/// </summary>
	/// <param name = "opne">4</param>
	public void SetOneOpneClose(bool opne)
	{
		one_opneclose_ = opne;
	}
}

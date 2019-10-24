using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustManager : MonoBehaviour
{
	/// <summary>
	/// 全部の箱が開いて時間で開いたり閉じたりするモード
	/// </summary>
	// それぞれのごみ箱を管理
	[Header("ごみ箱の数")]
	[SerializeField]
	List<Dust> dust_script_;

	// それぞれのごみ箱に時間を与える
	[Header("ごみ箱の開ける時間（各々）")]
	[SerializeField]
	List<float> limit_time_;

	/// <summary>
	/// 一つが閉じたら他が開くモード
	/// </summary>
	// それぞれのごみ箱に一緒の時間を与える
	[Header("ごみ箱の開ける時間（全部一緒）")]
	[SerializeField]
	float oneopen_time_;

	// 閉じたかどうか
	bool dust_close_ = false;

	// 空いているごみ箱の番号
	int number_ = 0;

	// 前に開けたごみ箱
	int old_number_ = -1;

	/// <summary>
	/// デバッグモード
	/// </summary>
	[Header("全部の箱が開いて時間で開いたり閉じたりするモード")]
	[SerializeField]
	bool all_opneclose_ = true;

	[Header("一つが閉じたら他が開くモード")]
	[SerializeField]
	bool one_opneclose_ = false;

	void Start()
    {
	}

	private void Awake()
	{
		for (int i = 0; i < this.dust_script_.Count; i++)
		{
			// それぞれのごみ箱に開く時間をセット
			this.dust_script_[i].SetLimitTime(this.limit_time_[i]);
			this.dust_script_[i].SetOneOpenTime(this.oneopen_time_);

			// デバッグモード
			this.dust_script_[i].SetAllOpneClose(all_opneclose_);
			this.dust_script_[i].SetOneOpneClose(one_opneclose_);
		}
	}

	private void Update()
	{
		// 一つが閉じたら他が開くモード
		if (one_opneclose_)
		{
			OnesOpenClose();
		}
	}

	/// <summary>
	/// 一つが閉じたら他が開くモード
	/// </summary>
	private void OnesOpenClose()
	{
		// ごみ箱が閉じていたら
		if (this.dust_close_)
		{
			//number_ = Enumerable.Range(0, this.dust_script_.Count).OrderBy(n => Guid.NewGuid()).Take(1).ToArray();
			// 同じごみ箱は開けない
			while (number_ == old_number_)
			{
				number_ = Random.Range(0, this.dust_script_.Count);
			}
			old_number_ = number_;
			this.dust_script_[number_].SetGetOpenDust = true;
			this.dust_close_ = false;
		}
		else
		{
			// ごみ箱が閉じたら
			if (!this.dust_script_[number_].SetGetOpenDust)
			{
				this.dust_close_ = true;
			}
		}
	}
}

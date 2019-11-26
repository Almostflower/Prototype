using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightGauge : BaseMonoBehaviour
{
	/// <summary>
	/// プレイヤー
	/// </summary>
	[SerializeField]
	private Player player_;

	/// <summary>
	/// 画像
	/// </summary>
	[SerializeField]
	private Image background_;

	/// <summary>
	/// アイコンを隠す時間
	/// </summary>
	[SerializeField]
	private float hydetime_ = 0;

	private float count_ = 0;

	private void awake()
	{
		base.Awake();
	}

	void Start()
    {
		background_.enabled = false;
		count_ = 0;
	}

	public override void UpdateNormal()
	{
		// うさぎを持ったら
		if (player_.GripFlag)
		{
			background_.enabled = true;
			count_ = 0;
			background_.fillAmount = player_.HoldingTimeCounter / player_.HoldingTime;
		}
		else
		{
			if (background_.fillAmount < 1)
			{
				background_.fillAmount += 0.1f;
			}
			else
			{
				count_ += Time.deltaTime;
			}

			if (count_ > hydetime_)
			{
				background_.enabled = false;
			}
		}
	}
}

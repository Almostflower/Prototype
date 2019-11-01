using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
	Slider slider_;

	[SerializeField]
	Timer timer_time_;

	private float hp_ = 0;

	void Start()
	{
		// スライダーを取得する
		slider_ = GameObject.Find("Slider").GetComponent<Slider>();

		slider_.maxValue = timer_time_.minute_ * 60 + timer_time_.seconds_;
	}


	void Update()
	{
		// HPゲージに値を設定
		slider_.value = timer_time_.total_time_;
	}
}

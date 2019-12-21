using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultGauge : BaseMonoBehaviour
{
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
    /// 減算する値
    /// </summary>
    private float gauge_value_ = 0;
    public float GaugeValue
    {
		get { return gauge_value_; }
        set { gauge_value_ = value; }
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
    }

    public override void UpdateNormal()
    {      
		// ゲージに値を設定
        slider_.value = gauge_value_;

        // 背景のゲージ
        background_.fillAmount = gauge_value_ / slider_.maxValue + (1 - gauge_value_ / slider_.maxValue) * 0.1f;
    }

	public void SetMaxValue(int maxval)
	{
		slider_.maxValue = maxval;
	}
}

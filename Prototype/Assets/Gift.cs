﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : BaseMonoBehaviour
{
	/// <summary>
	/// デバッグモード
	/// </summary>
	[SerializeField]
	private bool debug_mode_ = false;
	public bool DebugMode
	{
		set { debug_mode_ = value; }
	}

	///////////////////////////////////////////////////////////////////
	/// プロトを見て変える
	/// <summary>
	/// 1本のゲージで見せるモード
	/// </summary>
	[SerializeField]
	private bool debug_one_time_ = false;
	public bool DebugOneTime
	{
		set { debug_one_time_ = value; }
	}
	///////////////////////////////////////////////////////////////////

	/// <summary>
	/// 不良品になるまでのリミットタイム
	/// </summary>
	[SerializeField]
    private float BadLimitTime = 10.0f;
	//public float badLimitTime
	//{
	//	get { return BadLimitTime; }
	//}

	/// <summary>
	/// 消えるまでのリミットタイム
	/// </summary>
	[SerializeField]
    private float DustLimitTime = 10.0f;
	//public float dustLimitTime
	//{
	//	get { return DustLimitTime; }
	//}

	/// <summary>
	/// ギフトが粗悪になってある一定時間すぎた時　true
	/// </summary>
	private bool DustFlag;

    /// <summary>
    /// 自然消滅
    /// </summary>
    private bool DeathFlag;

    /// <summary>
    /// プレイヤーがギフトを運んでいるかのチェック
    /// </summary>
    private bool playerCarryFlag;
    public bool PlayerCarryFlag
    {
        get { return playerCarryFlag; }
        set { playerCarryFlag = value; }
    }

	/// <summary>
	/// 主なリミットタイム
	/// </summary>
	private float mastertime;
	public float MasterTime
	{
		get { return mastertime; }
	}

	/// <summary>
	/// Ui表示
	/// </summary>
	[SerializeField]
	private Text timer_text_;

	/// <summary>
	/// ゲージスクリプト
	/// </summary>
	[SerializeField]
	private Gauge gauge_;

	/// <summary>
	/// 一回きりのフラグ
	/// </summary>
	private bool once_ = false;

	/// <summary>
	/// BaseMonoBehaviourの初期化
	/// </summary>
	protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ギフトの初期化
    /// </summary>
    void Start()
    {
        DustFlag = false;
        DeathFlag = false;
        PlayerCarryFlag = false;
		once_ = false;
		mastertime = BadLimitTime;
		if(debug_one_time_)
		{
			mastertime = BadLimitTime + DustLimitTime;
		}
    }

    /// <summary>
    /// ギフトの更新
    /// </summary>
    public override void UpdateNormal()
    {
		if (!DustFlag && BadLimitTime > 0.0f)
		{
			// ギフトの良い状態の更新
			BadLimitTime -= Time.deltaTime;
		}
		else if (DustFlag && !playerCarryFlag)
		{
			// ギフトの悪い状態の更新
			DustLimitTime -= Time.deltaTime;
		}

		if (!once_)
		{
			gauge_.SetMaxValue(mastertime, DustLimitTime, debug_one_time_, true);
			once_ = true;
		}

		// ギフトの時間の更新
		mastertime -= Time.deltaTime;
		// ギフトの時間をゲージに渡す
		gauge_.TimerValue = mastertime;

		if (debug_mode_)
		{
			timer_text_.enabled = true;
			// タイマー表示用UIテキストに時間を表示する
			timer_text_.text = mastertime.ToString("F2");
		}
		else
		{
			timer_text_.enabled = false;
		}

		// 良い状態から悪い状態への条件判定
		if (BadLimitTime < 0.0f)
        {
            if (gameObject.tag != "Bad gift")
            {
                gameObject.tag = "Bad gift";
                Debug.Log("ギフトが悪くなった");
                this.GetComponent<Renderer>().material.color = Color.red;
				if(!debug_one_time_)
				{
					mastertime = DustLimitTime;
					gauge_.SetGiftValue(mastertime);
				}
            }

            DustFlag = true;
        }

        // 悪い状態から自然消滅への条件判定
        if (/*!GameStatusManager.Instance.GetLiftGift() && */DustFlag && DustLimitTime < 0.0f)
        {
            DeathFlag = true;
            Debug.Log("ギフト消滅");
        }
    }

    /// <summary>
    /// ギフトの状態が悪いか判定
    /// </summary>
    public bool GetDustFlag()
    {
        return DustFlag;
    }

    /// <summary>
    /// ギフトの状態が消滅していいか判定
    /// </summary>
    public bool GetDeathFlag()
    {
        return DeathFlag;
    }

}
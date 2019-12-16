using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : BaseMonoBehaviour
{
    [SerializeField]
    GameObject Gift01, Gift02, Gift03;
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
    private bool debug_one_time_ = false;
    public bool DebugOneTime
    {
        set { debug_one_time_ = value; }
    }
	///////////////////////////////////////////////////////////////////

	/// <summary>
	/// 悪いスコア
	/// </summary>
	private int badscore_ = 0;

    /// <summary>
    /// 不良品になるまでのリミットタイム
    /// </summary>
    private float badLimitTime = 10.0f;
    public float GetBadLimitTime
    {
        get { return badLimitTime; }
        set { badLimitTime = value; }
    }

    /// <summary>
    /// 消えるまでのリミットタイム
    /// </summary>
    private float dustLimitTime = 10.0f;
    public float GetDustLimitTime
    {
        get { return dustLimitTime; }
        set { dustLimitTime = value; }
    }

    /// <summary>
    /// ギフトが粗悪になってある一定時間すぎた時　true
    /// </summary>
    private bool DustFlag;

    /// <summary>
    /// 自然消滅
    /// </summary>
    private bool DeathFlag;

    public bool SetGetDeathFlag
    {
        get { return DeathFlag; }
        set { DeathFlag = value; }
    }
    /// <summary>
    /// プレイヤーがギフトを運んでいるかのチェック
    /// </summary>
    private bool playerAbsorbFlag;
    public bool PlayerAbsorbFlag
    {
        get { return playerAbsorbFlag; }
        set { playerAbsorbFlag = value; }
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
    private GiftGauge gauge_;

	/// <summary>
	/// スコアスクリプト
	/// </summary>
	private Score socre_;

    /// <summary>
    /// 一回きりのフラグ
    /// </summary>
    private bool once_ = false;

	/// <summary>
	/// 良いアイコン
	/// </summary>
	[SerializeField]
	private GameObject GoodIcon;

	/// <summary>
	/// 悪いアイコン
	/// </summary>
	[SerializeField]
	private GameObject BadIcon;

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
        playerAbsorbFlag = false;
        once_ = false;
        mastertime = badLimitTime;
		badscore_ *= -1;
		socre_ = GameObject.Find("Score").GetComponent<Score>();
		BadIcon.SetActive(false);

		if (debug_one_time_)
        {
            mastertime = badLimitTime + dustLimitTime;
        }

        // パーティクル制御
        this.transform.GetChild(6).gameObject.SetActive(true);
        this.transform.GetChild(7).gameObject.SetActive(false);

    }

    /// <summary>
    /// ギフトの更新
    /// </summary>
    public override void UpdateNormal()
    {
        if (SceneStatusManager.Instance.PauseButton == 1)
        {
            Debug.Log("nowTime bad" + badLimitTime);
            Debug.Log("nowTime dust" + dustLimitTime);
            if (!once_)
            {
                //gauge_.SetMaxValue(mastertime, dustLimitTime, debug_one_time_, true);
                gauge_.SetMaxValue(mastertime, dustLimitTime, debug_one_time_);
                once_ = true;
            }

            if (!DustFlag && badLimitTime > 0.0f)
            {
                // ギフトの良い状態の更新
                badLimitTime -= Time.deltaTime;
            }
            else if (DustFlag && !playerAbsorbFlag)
            {
                // ギフトの悪い状態の更新
                dustLimitTime -= Time.deltaTime;
            }

            // ギフトの時間の更新
            mastertime -= Time.deltaTime;
            // ギフトの時間をゲージに渡す
            gauge_.GaugeValue = mastertime;

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
            if (badLimitTime < 0.0f)
            {
                if (gameObject.tag != "Bad gift")
                {
                    gameObject.tag = "Bad gift";
                    Debug.Log("ギフトが悪くなった");
                    Gift01.GetComponent<Renderer>().material.color = Color.gray;
                    Gift02.GetComponent<Renderer>().material.color = Color.gray;
                    Gift03.GetComponent<Renderer>().material.color = Color.gray;
                    GoodIcon.SetActive(false);
                    BadIcon.SetActive(true);
                    // パーティクル制御
                    this.transform.GetChild(6).gameObject.SetActive(false);
                    this.transform.GetChild(7).gameObject.SetActive(true);
                    if (!debug_one_time_)
                    {
                        mastertime = dustLimitTime;
                        gauge_.SetMaxValue(mastertime);
                    }
                }

                DustFlag = true;
            }

            // 悪い状態から自然消滅への条件判定
            if (/*!GameStatusManager.Instance.GetLiftGift() && */DustFlag && dustLimitTime < 0.0f)
            {               
                DeathFlag = true;
                socre_.SetScore(badscore_);
                Debug.Log("ギフト消滅");
            }
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

	/// <summary>
	/// ギフトの初期設定
	/// </summary>
	/// <param name="badscore"></param>
	/// <param name="badlimittime"></param>
	/// <param name="dustlimittime"></param>
	public void SetGift(int badscore, float badlimittime, float dustlimittime)
	{
		badscore_ = badscore;
		badLimitTime = badlimittime;
		dustLimitTime = dustlimittime;
	}
}
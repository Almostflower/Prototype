﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Player : BaseMonoBehaviour
{
    [SerializeField]
    private GameObject footPrintPrefab;
    float foottime = 0;
    [SerializeField]
    private Animator PlayerAnimator;
    [SerializeField]
    private CharacterController PlayerController;
    [SerializeField]
    private float Speed = 5f;
    [SerializeField]
    private float DefaultSpeed;
    [SerializeField]
    private float RunSpeed;
    private Vector3 velocity;
    private string PlayerActionParameter = "Move";
    private float hit;
    private Vector3 direction;
    [SerializeField]
    private GameObject GiftArea;//プレイヤーの子要素にギフトを持った時の位置指定してアタッチさせるために必要な変数。

    [SerializeField]
    private float rotateSpeed = 120f;

    [SerializeField]
    private int giftMaxNum;

    [SerializeField] private GameObject[] abarerukun;

    //加速フラグ
    bool speedflag = false;
    //コントローラーの縦方向の傾きを取得する変数
    [SerializeField]
    private GameObject SparkParticle;
    Vector3 Direction;
    /// <summary>
    /// 所有ギフトの情報
    /// </summary>
    private float[] giftTime;
    private bool[] giftType;

    ///<summary>
    ///プレイヤーの足元座標
    ///</summary>
    [SerializeField, Tooltip("足元座標")]
    private Transform footpos;
    /// <summary>
    /// 良いギフトの所有数
    /// </summary>
    [SerializeField, Tooltip("良いギフトの数")] private int goodGiftNum;
    public int GoodGiftNum
    {
        get { return goodGiftNum; }
        set { goodGiftNum = value; }
    }

    /// <summary>
    /// 悪いギフトの所有数
    /// </summary>
    [SerializeField, Tooltip("悪いギフトの数")] private int badGiftNum;
    public int BadGiftNum
    {
        get { return badGiftNum; }
        set { badGiftNum = value; }
    }

    /// <summary>
    /// ウサギのマネージャー
    /// </summary>
    [SerializeField] private GameObject rabbitManager;

    /// <summary>
    /// 持っているウサギの番号
    /// </summary>
    private int holdingRabbitNumber;

    /// <summary>
    /// ウサギを持ち続ける
    /// </summary>
    [SerializeField] private bool holdingRabbitFlag;
	public bool HoldingRabbitFlag
	{
		get { return holdingRabbitFlag; }
	}

    /// <summary>
    /// ウサギを持つ
    /// </summary>
    [SerializeField] private bool gripFlag;
    public bool GripFlag
    {
        get { return gripFlag; }
    }

    /// <summary>
    /// 握力時間
    /// </summary>
    [SerializeField] private float holdingTime;
	public float HoldingTime
	{
		get { return holdingTime; }
	}

    /// <summary>
    /// 握力経過時間
    /// </summary>
    private float holdingTimeCounter;
	public float HoldingTimeCounter
	{
		get { return holdingTimeCounter; }
	}

    /// <summary>
    /// スコアのスクリプト
    /// </summary>
    [SerializeField]
	private Score score;

    private void awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        goodGiftNum = 0;
        badGiftNum = 0;
        giftTime = new float[giftMaxNum];
        giftType = new bool[giftMaxNum];
        holdingRabbitFlag = false;
        holdingTimeCounter = 0;
        holdingRabbitNumber = -1;
        
        SparkParticle.SetActive(false);

        gripFlag = false;

        abarerukun[0].SetActive(false);
        abarerukun[1].SetActive(false);
    }
    IEnumerator Disappearing()
    {
        int step = 90;
        for (int i = 0; i < step; i++)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1 - 1.0f * i / step);
            yield return null;
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    public override void UpdateNormal()
    {
		this.foottime += Time.deltaTime;
        if (this.foottime > 0.35f)
        {
            this.foottime = 0;
            Instantiate(footPrintPrefab, footpos.position, transform.rotation);//
        }

        // ウサギとの動作
        if (!holdingRabbitFlag && !gripFlag)
        {
            CheckCarryRabbit();
        }
        else if (holdingRabbitFlag && !gripFlag)
        {
            TransferGift();
        }
        else if (gripFlag)
        {
            CarryRabbit();
        }

        PlayerMove();

        // ギフト所持数の更新
        this.transform.GetChild(6).GetChild(0).gameObject.GetComponent<Text>().text = "良いGiftの数：" + goodGiftNum.ToString();
        this.transform.GetChild(6).GetChild(1).gameObject.GetComponent<Text>().text = "悪いGiftの数：" + badGiftNum.ToString();
    }
    private void PlayerMove()
    {
        //velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if(speedflag)
        {
            if (Speed < RunSpeed)
            {
                Speed += 0.1f;
            }
        }
        else
        {
            if(Speed > DefaultSpeed)
            {
                Speed -= 0.1f;
            }
        }


        if (PlayerController.isGrounded)
        {
            Direction = (transform.forward * Input.GetAxis("Vertical")) * Speed * Time.fixedDeltaTime;
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime, 0);
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                //走るアニメーション速度変更
                PlayerAnimator.SetFloat("Speed", 1.5f);
                SparkParticle.SetActive(true);
                speedflag = true;//加速時
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                //走るアニメーション速度変更
                PlayerAnimator.SetFloat("Speed", 0.8f);
                SparkParticle.SetActive(false);
                speedflag = false;//通常時
            }
            if (Input.GetAxis("Vertical") != 0f)
            {
                PlayerAnimator.SetFloat(PlayerActionParameter, velocity.magnitude);
            }

            if (Input.GetAxis("Horizontal") != 0f)
            {
                PlayerAnimator.SetFloat(PlayerActionParameter, velocity.magnitude);
            }

            if (Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f)
            {
                PlayerAnimator.SetFloat(PlayerActionParameter, 0f);
            }
        }
        else
        {
            Direction.y += Physics.gravity.y * Time.deltaTime;
        }


        velocity.y += Physics.gravity.y * Time.deltaTime;

        PlayerController.Move(Direction);
    }

    /// <summary>
    /// ウサギを持ち上げられるか確認
    /// </summary>
    private void CheckCarryRabbit()
    {
        for (int i = 0; i < rabbitManager.GetComponent<RabbitManager>().RabbitMaxNum; i++)
        {
            // ボタン入力
            if (Input.GetKeyDown(KeyCode.Space) && rabbitManager.GetComponent<RabbitManager>().rabbitManager[i].GetComponent<RabbitScript>().HitPlayer)
            {
                // ウサギキャッチSE
                SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);

                holdingRabbitFlag = true;
                holdingTimeCounter = holdingTime;
                holdingRabbitNumber = i;
                break;
            }
        }

    }

    /// <summary>
    /// ギフトをウサギに送る
    /// </summary>
    private void TransferGift()
    {
        // 良いギフトがあり良いうさぎなら
        if (goodGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Good)
        {
            // スコアを反映
            for (int i = 0; i < badGiftNum + goodGiftNum; i++)
            {
                if (giftType[i])
                {
                    score.SetScore((int)giftTime[i]);
                    giftTime[i] = 0;
                }
            }

            // ウサギを消す
            goodGiftNum = 0;

        }
        else if (badGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Bad)
        {
            // 悪いギフトがあり悪いうさぎなら

            // スコアを反映
            for (int i = 0; i < badGiftNum + goodGiftNum; i++)
            {
                if (!giftType[i])
                {
                    score.SetScore((int)giftTime[i]);
                    giftTime[i] = 0;
                }
            }

            // ウサギを消す
            badGiftNum = 0;
        }
        else
        {
            // ウサギを運ぶ状態へ変更
            gripFlag = true;

            // 暴れる君を生成
            if (rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Good)
            {
                abarerukun[0].SetActive(true);
            }
            else
            {
                abarerukun[1].SetActive(true);
            }

        }

        holdingRabbitFlag = false;
        rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;

    }

    /// <summary>
    /// ウサギを運んでいる
    /// </summary>
    private void CarryRabbit()
    {
        // 握力ゲージの処理
        if (Input.GetKey(KeyCode.Space))
        {
            holdingTimeCounter -= Time.deltaTime;
            if (holdingTimeCounter <= 0)
            {
                gripFlag = false;

                // あばれる君を削除
                abarerukun[0].SetActive(false);
                abarerukun[1].SetActive(false);
            }
        }
        else
        {
            gripFlag = false;

            // あばれる君を削除
            abarerukun[0].SetActive(false);
            abarerukun[1].SetActive(false);
        }
    }

    /// <summary>
    /// ギフトに触れた時状態によって、取得もしくは持ち上げるようにさせる
    /// </summary>
    /// <param name="other">判定の対象物</param>
    private void OnTriggerEnter(Collider other)
    {
        // ギフト（良）の所有数がオーバーしていないかチェック
        if (goodGiftNum + badGiftNum < giftMaxNum)
        {
			//普通のギフトの時に当たったら、取得したエフェクト発生させて、ゲージのパラメーターが増加し、ギフト消去させる
			if (other.gameObject.tag == "gift")
            {
				// ギフト回収SE
				SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.GetGift_SE);

				//other.gameObject.GetComponent<Gift>().GoodFlag = true;
				// プレイヤーが吸収
				//Destroy(other.gameObject);

				// プレイヤーがギフト吸収したことを知らせる
				other.gameObject.GetComponent<Gift>().PlayerAbsorbFlag = true;
                // 取得したギフトの情報を保存
                giftTime[goodGiftNum + badGiftNum] = other.gameObject.GetComponent<Gift>().GetBadLimitTime;
                giftType[goodGiftNum + badGiftNum] = other.gameObject.GetComponent<Gift>().GetDustFlag() ^ true;

                // ギフト数追加
                goodGiftNum++;
            }
            //悪いギフトに当たったら、キャラクタが持ち上げるように位置を変更させ移動できるようにする。
            if (other.gameObject.tag == "Bad gift")
            {
				// ギフト回収SE
				SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.GetGift_SE);

				Debug.Log("あたってる");
                GameStatusManager.Instance.SetLiftGift(true);
                //other.gameObject.GetComponent<Gift>().PlayerCarryFlag = true;   // ギフトを運ぶ
                Vector3 m = GiftArea.transform.position;
                other.transform.position = m;
                other.transform.parent = GiftArea.transform;
                other.transform.rotation = Quaternion.identity;

                // プレイヤーがギフト吸収したことを知らせる
                other.gameObject.GetComponent<Gift>().PlayerAbsorbFlag = true;
                // 取得したギフトの情報を保存
                giftTime[goodGiftNum + badGiftNum] = other.gameObject.GetComponent<Gift>().GetDustLimitTime;
                giftType[goodGiftNum + badGiftNum] = other.gameObject.GetComponent<Gift>().GetDustFlag() ^ true;

                // ギフト数追加
                badGiftNum++;
            }
        }

        // うさぎと当たっているかチェック
        if(other.gameObject.tag == "Rabbit")
        {
            other.gameObject.GetComponent<RabbitScript>().HitPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Rabbit")
        {
            other.gameObject.GetComponent<RabbitScript>().HitPlayer = false;
        }
    }
}

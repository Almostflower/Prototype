using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Player : BaseMonoBehaviour
{
    [SerializeField]
    private ParticleSystem GoodParticle;
    [SerializeField]
    private ParticleSystem BadParticle;

    private bool GoodParticleFlag;
    private bool BadParticleFlag;

    private float particletime;
    //[SerializeField]
    //private GameObject footPrintPrefab;
    //float foottime = 0;
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
    /// スコアのスクリプト
    /// </summary>
    [SerializeField]
    private Score score;

    /// <summary>
    /// UIのギフト表示
    /// </summary>
    [SerializeField]
    private List<Number> number_ = new List<Number>();

    /// <summary>
    ///	スタミナの最大値
    /// </summary>
    [SerializeField]
    private float staminamax = 0;
    public float StaminaMax
    {
        get { return staminamax; }
    }

    /// <summary>
    /// スタミナ現在量
    /// </summary>
    private float stamina = 0;
    public float Stamina
    {
        get { return stamina; }
    }

    /// <summary>
    /// スタミナを回復する速度
    /// </summary>
    [SerializeField]
    private float staminarecoveryspeed = 0;

    /// <summary>
    /// スタミナ減少する速度
    /// </summary>
    [SerializeField]
    private float staminaspeed = 0;

    /// <summary>
    /// 走っているかのフラグ
    /// </summary>
    private bool dashflag = false;

	/// <summary>
	/// スタート準備
	/// </summary>
	[SerializeField]
	private StartReady start_ready_;

	private bool readyFlag;

    enum UIGfit
    {
        GiftGood,
        GiftBad,
        Max
    }

    private void awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        GoodParticleFlag = false;
        BadParticleFlag = false;
        particletime = 0.0f;
        goodGiftNum = 0;
        badGiftNum = 0;
        giftTime = new float[giftMaxNum];
        giftType = new bool[giftMaxNum];
        stamina = 0;
        dashflag = false;

        SparkParticle.SetActive(false);
        BadParticle.Stop();
        GoodParticle.Stop();

		readyFlag = false;

        SceneStatusManager.Instance.GameReady = true;

    }
    //IEnumerator Disappearing()
    //{
    //    int step = 90;
    //    for (int i = 0; i < step; i++)
    //    {
    //        GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1 - 1.0f * i / step);
    //        yield return null;
    //    }
    //    Destroy(gameObject);
    //}
    float pauseresettime = 0.0f;
    bool pauseflag = false;
    // Update is called once per 

    /// <summary>
    /// プレイヤーの更新
    /// </summary>
    public override void UpdateNormal()
    {
		if (!readyFlag)
		{
			// フェード完了しているか
			if(SceneStatusManager.Instance.GameReady)
			{
				start_ready_.SetStart = true;
			}

			if(start_ready_.GetGo)
			{
				readyFlag = true;
			}

			return;
		}

        if(GoodParticleFlag || BadParticleFlag)
        {
            if(GoodParticleFlag)
            {
                particletime += Time.deltaTime;
            }
            if(BadParticleFlag)
            {
                particletime += Time.deltaTime;
            }

            if(particletime > 2.0f)
            {
                GoodParticleFlag = false;
                BadParticleFlag = false;
                particletime = 0.0f;
                GoodParticle.gameObject.SetActive(false);
                BadParticle.gameObject.SetActive(false);
            }

        }

        //this.foottime += Time.deltaTime;
        //if (this.foottime > 0.35f)
        //{
        //    this.foottime = 0;
        //    Instantiate(footPrintPrefab, footpos.position, transform.rotation);//
        //}

        // ポーズ
        if (pauseflag)
        {
            pauseresettime += Time.deltaTime;
            if(pauseresettime > 0.5f)
            {
                pauseresettime = 0.0f;
                pauseflag = false;
            }
        }

        // 
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Joystick1Button12) && !pauseflag)
        {
            pauseflag = true;
            SceneStatusManager.Instance.PauseButton *= -1;
        }

        // ウサギとのやり取り
        if(SceneStatusManager.Instance.PauseButton == 1)
        {
            PlayerAnimator.enabled = true;

            // ウサギをつかんでいない状態
            CheckCarryRabbit();

            PlayerMove();

			// ギフト所持数の更新
			number_[(int)UIGfit.GiftGood].SetNum(goodGiftNum);
			number_[(int)UIGfit.GiftBad].SetNum(badGiftNum);
        }
        else
        {
            PlayerAnimator.enabled = false;
        }
    }

    /// <summary>
    /// 現在の回転方向のベクトル
    /// </summary>
    private Vector3 currentRotateForward = Vector3.zero;

    /// <summary>
    /// キャラクターの回転を行う
    /// </summary>
    protected void DoRotate()
    {
        // 現在の移動ベクトルを取得
        Vector3 moveVelocity = PlayerController.velocity;
        moveVelocity.y = 0;

        // 移動ベクトルが零ベクトル以外の場合は回転用ベクトルに設定
        if (moveVelocity != Vector3.zero)
        {
            currentRotateForward = moveVelocity;
        }

        // 角度と回転方向を取得
        float value = Mathf.Min(1, Speed * Time.deltaTime / Vector3.Angle(transform.forward, currentRotateForward));
        Vector3 newForward = Vector3.Slerp(transform.forward, currentRotateForward, value);

        if (newForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(newForward, transform.up);
        }
    }
    float animflame;
    private void PlayerMove()
    {
        if (Input.GetAxis("Vertical") == 0.0f)
        {
            PlayerAnimator.SetFloat(PlayerActionParameter, 0.0f);
        }
        else
        {
            animflame = Input.GetAxis("Vertical");
            PlayerAnimator.SetFloat(PlayerActionParameter, animflame);
        }
        if (speedflag)
        {
            //スタミナ減少
            stamina -= staminaspeed;

            //スタミナが0以下じゃないなら
            if (stamina > 0)
            {
                //走るアニメーション速度変更
                PlayerAnimator.SetFloat("Speed", 2.5f);
                SparkParticle.SetActive(true);
            }
            else
            {
                //走るアニメーション速度変更
                PlayerAnimator.SetFloat("Speed", 1.0f);
                SparkParticle.SetActive(false);
                speedflag = false;//通常時
                dashflag = false;//通常時
                stamina = 0;
            }

            if (Speed < RunSpeed)
            {
                Speed += 0.1f;
            }
        }
        else
        {
            if (Speed > DefaultSpeed)
            {
                Speed -= 0.1f;
            }
        }

        if (Input.GetAxis("Vertical") <= 0.002924493f)
        {
            //走るアニメーション速度変更
            PlayerAnimator.SetFloat("Speed", 1.0f);
            SparkParticle.SetActive(false);
            speedflag = false;//通常時
            dashflag = false;//通常時
            stamina = 0.0f;
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0.002924493f || Input.GetKey(KeyCode.JoystickButton1) && Input.GetAxis("Vertical") > 0.002924493f)
        {
            //走るアニメーション速度変更
            PlayerAnimator.SetFloat("Speed", 1.0f);
            SparkParticle.SetActive(false);
            speedflag = false;//通常時
            dashflag = false;//通常時

            //スタミナを回復
            stamina += staminaspeed;

            //スタミナが最大なら
            if (stamina >= staminamax)
            {
                stamina = staminamax;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.JoystickButton1))
        {
            speedflag = true;//加速時
            dashflag = true;//加速時
            //Debug.LogError("話した");
        }


        if (PlayerController.isGrounded)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime, 0);          

            if (Input.GetAxis("Vertical") >= 0.0f)
            {
                PlayerAnimator.SetFloat("Speed", 1.0f);
                PlayerAnimator.SetBool("Back", false);
                //前進んでいるとき
                Direction = (transform.forward * Input.GetAxis("Vertical")) * Speed * Time.fixedDeltaTime;               
            }
            else
            {
                //後ろ下がっているとき
                if (!dashflag)
                {
                    PlayerAnimator.SetBool("Back", true);
                    SparkParticle.SetActive(false);
                    PlayerAnimator.SetFloat(PlayerActionParameter, -0.1f);
                    Direction = (transform.forward * Input.GetAxis("Vertical")) * (Speed * 0.3f) * Time.fixedDeltaTime;
                }
            }
        }
        else
        {
            Direction.y += Physics.gravity.y * Time.deltaTime;
        }


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
            if (Input.GetKeyDown(KeyCode.Space) && rabbitManager.GetComponent<RabbitManager>().rabbitManager[i].GetComponent<RabbitScript>().HitPlayer 
                || Input.GetKeyDown(KeyCode.Joystick1Button0) && rabbitManager.GetComponent<RabbitManager>().rabbitManager[i].GetComponent<RabbitScript>().HitPlayer)
            {
                // ウサギキャッチSE
                SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);

                // ギフトをウサギに送る
                TransferGift(i);
                break;
            }
        }

    }

    /// <summary>
    /// ギフトをウサギに送る
    /// </summary>
    private void TransferGift(int rabbitNum)
    {
        // 良いギフトがあり良いうさぎなら
        if (goodGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[rabbitNum] == RabbitManager.RabbitType.Good)
        {
            GoodParticleStart();
            // スコアを反映
            for (int i = 0; i < badGiftNum + goodGiftNum; i++)
            {
                if (giftType[i])
                {
                    score.SetScore((int)giftTime[i], (int)Score.GIFTSTATUS.giftgood);
                    giftTime[i] = 0;
                }
            }

            // スコアに良いウサギの情報を渡す
            score.SetRabbitGood();

            // ウサギを消す
            goodGiftNum = 0;

        }
        else if (badGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[rabbitNum] == RabbitManager.RabbitType.Bad)
        {
            // 悪いギフトがあり悪いうさぎなら
            BadParticleStart();
            // スコアを反映
            for (int i = 0; i < badGiftNum + goodGiftNum; i++)
            {
                if (!giftType[i])
                {
                    score.SetScore((int)giftTime[i], (int)Score.GIFTSTATUS.giftbad);
                    giftTime[i] = 0;
                }
            }

            // スコアに悪いウサギの情報を渡す
            score.SetRabbitBad();
            // ウサギを消す
            badGiftNum = 0;
        }

        // ウサギをDEADに変更
        rabbitManager.GetComponent<RabbitManager>().rabbitManager[rabbitNum].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;
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
            if (SceneStatusManager.Instance.PauseButton == 1)
            {
                //普通のギフトの時に当たったら、取得したエフェクト発生させて、ゲージのパラメーターが増加し、ギフト消去させる
                if (other.gameObject.tag == "gift")
                {
                    GoodParticleStart();
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
                    BadParticleStart();
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
        }

        // うさぎと当たっているかチェック
        if (other.gameObject.tag == "Rabbit")
        {
            if (SceneStatusManager.Instance.PauseButton == 1)
            {
                other.gameObject.GetComponent<RabbitScript>().HitPlayer = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Rabbit")
        {
            other.gameObject.GetComponent<RabbitScript>().HitPlayer = false;
        }
    }

    public void SetDirection(Vector3 pos)
    {
        PlayerController.Move(pos);
    }
    //public void GoodParticleStart()
    //{
    //    ParticleSystem Goodparticles = Instantiate(GoodParticle, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 1.0f), Quaternion.identity);
    //    Destroy(Goodparticles, 2.0f);
    //    return;
    //}
    //
    //public void BadParticleStart()
    //{
    //    ParticleSystem Badparticles = Instantiate(BadParticle, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 1.0f), Quaternion.identity);
    //    Destroy(Badparticles, 2.0f);
    //    return;
    //}
    public void GoodParticleStart()
    {
        GoodParticle.gameObject.SetActive(true);
        GoodParticle.gameObject.transform.position =  new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 1.0f);
        GoodParticle.Play();
        return;
    }

    public void BadParticleStart()
    {
        BadParticle.gameObject.SetActive(true);
        BadParticle.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 1.0f);
        BadParticle.Play();
        return;
    }
}

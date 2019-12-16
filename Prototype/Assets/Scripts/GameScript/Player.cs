using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Player : BaseMonoBehaviour
{
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

    /// <summary>
    /// UIのギフト表示
    /// </summary>
    [SerializeField]
    private List<ImageNo> image_ = new List<ImageNo>();

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
        goodGiftNum = 0;
        badGiftNum = 0;
        giftTime = new float[giftMaxNum];
        giftType = new bool[giftMaxNum];
        holdingRabbitFlag = false;
        holdingTimeCounter = 0;
        holdingRabbitNumber = -1;
        stamina = 0;
        dashflag = false;

        SparkParticle.SetActive(false);

        gripFlag = false;
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
    float pauseresettime = 0.0f;
    bool pauseflag = false;
    // Update is called once per frame
    public override void UpdateNormal()
    {
        //this.foottime += Time.deltaTime;
        //if (this.foottime > 0.35f)
        //{
        //    this.foottime = 0;
        //    Instantiate(footPrintPrefab, footpos.position, transform.rotation);//
        //}
        if(pauseflag)
        {
            pauseresettime += Time.deltaTime;
            if(pauseresettime > 0.5f)
            {
                pauseresettime = 0.0f;
                pauseflag = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Joystick1Button5) && !pauseflag)
        {
            pauseflag = true;
            SceneStatusManager.Instance.PauseButton *= -1;
        }
        if(SceneStatusManager.Instance.PauseButton == 1)
        {
            PlayerAnimator.enabled = true;
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
            image_[(int)UIGfit.GiftGood].SetNo(goodGiftNum);
            image_[(int)UIGfit.GiftBad].SetNo(badGiftNum);
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

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.JoystickButton1))
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
                || Input.GetKey(KeyCode.Joystick1Button0) && rabbitManager.GetComponent<RabbitManager>().rabbitManager[i].GetComponent<RabbitScript>().HitPlayer)
            {
                Debug.Log("きてます");
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
        holdingRabbitFlag = false;

        // 良いギフトがあり良いうさぎなら
        if (goodGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Good)
        {
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
        else if (badGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Bad)
        {
            // 悪いギフトがあり悪いうさぎなら

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
        else
        {
            // ウサギを運ぶ状態へ変更
            gripFlag = true;

        }

        rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;

    }

    /// <summary>
    /// ウサギを運んでいる
    /// </summary>
    private void CarryRabbit()
    {
        holdingTimeCounter -= Time.deltaTime;
        if (holdingTimeCounter <= 0)
        {
            gripFlag = false;
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
            if (SceneStatusManager.Instance.PauseButton == 1)
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
}

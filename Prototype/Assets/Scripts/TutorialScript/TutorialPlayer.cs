using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : BaseMonoBehaviour
{
    [SerializeField]
    private ParticleSystem GoodParticle;
    [SerializeField]
    private ParticleSystem BadParticle;

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
        BadParticle.Stop();
        GoodParticle.Stop();

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
        //this.foottime += Time.deltaTime;
        //if (this.foottime > 0.35f)
        //{
        //    this.foottime = 0;
        //    Instantiate(footPrintPrefab, footpos.position, transform.rotation);//
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    SceneStatusManager.Instance.PauseButton *= -1;
        //}
        if (SceneStatusManager.Instance.PauseButton == 1)
        {
            PlayerAnimator.enabled = true;

            //if (gripFlag)
            //{
            //    CarryRabbit();
            //}

            PlayerMove();

            // ギフト所持数の更新
            //image_[(int)UIGfit.GiftGood].SetNo(goodGiftNum);
            //image_[(int)UIGfit.GiftBad].SetNo(badGiftNum);
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
                if (SceneStatusManager.Instance.TutorialWarp)
                {
                    TutorialManagerScript.Instance.SetPhaseNumber(6);
                }
                stamina = staminamax;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)  || Input.GetKeyUp(KeyCode.JoystickButton1))
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
    /// ウサギを運んでいる
    /// </summary>
    //private void CarryRabbit()
    //{
    //    // 握力ゲージの処理
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        holdingTimeCounter -= Time.deltaTime;
    //        if (holdingTimeCounter <= 0)
    //        {
    //            gripFlag = false;

    //            // あばれる君を削除
    //            abarerukun[0].SetActive(false);
    //            abarerukun[1].SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        gripFlag = false;

    //        // あばれる君を削除
    //        abarerukun[0].SetActive(false);
    //        abarerukun[1].SetActive(false);
    //    }
    //}
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ChutorialGoodRabbit")
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0))
            {
                GoodParticleStart();
                SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
                TutorialManagerScript.Instance.SetPhaseNumber(2);
                Destroy(other.gameObject);
                //other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.tag == "ChutorialBadRabbit")
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button0))
            {
                BadParticleStart();
                SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
                TutorialManagerScript.Instance.SetPhaseNumber(4);
                Destroy(other.gameObject);
                //other.gameObject.SetActive(false);
            }
        }

        if(other.gameObject.tag == "TutorialWarp")
        {
            if(Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Joystick1Button2))
            {
                TutorialManagerScript.Instance.SetPhaseNumber(7);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ChutorialGoodGift")
        {
            GoodParticleStart();
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.GetGift_SE);
            TutorialManagerScript.Instance.SetPhaseNumber(1);
            //ChutorialScore.SetScore(1, (int)Score.GIFTSTATUS.giftgood);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "ChutorialBadGift")
        {
            BadParticleStart();
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.GetGift_SE);
            TutorialManagerScript.Instance.SetPhaseNumber(3);
            //ChutorialScore.SetScore(1, (int)Score.GIFTSTATUS.giftbad);
            Destroy(other.gameObject);
        }
    }

    public void SetDirection(Vector3 pos)
    {
        PlayerController.Move(pos);
    }

    public void GoodParticleStart()
    {
        ParticleSystem ps = Instantiate(GoodParticle, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 1.0f),Quaternion.identity);
        ps.gameObject.SetActive(true);
        Destroy(ps, 2.0f);
        return;
    }

    public void BadParticleStart()
    {
        ParticleSystem ps = Instantiate(BadParticle, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 1.0f), Quaternion.identity);
        ps.gameObject.SetActive(true);
        Destroy(ps, 2.0f);
        return;
    }
}

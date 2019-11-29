using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : BaseMonoBehaviour
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
    private float rotateSpeed = 120f;

    [SerializeField] private GameObject[] abarerukun;

    //加速フラグ
    bool speedflag = false;
    //コントローラーの縦方向の傾きを取得する変数
    [SerializeField]
    private GameObject SparkParticle;
    Vector3 Direction;

    ///<summary>
    ///プレイヤーの足元座標
    ///</summary>
    [SerializeField, Tooltip("足元座標")]
    private Transform footpos;

    [SerializeField]
    Score ChutorialScore;
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
    private void awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        holdingRabbitFlag = false;
        holdingTimeCounter = 0;
        
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

        if (gripFlag)
        {
            CarryRabbit();
        }

        PlayerMove();
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
    private void PlayerMove()
    {
        //velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (speedflag)
        {
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


        if (PlayerController.isGrounded)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime, 0);

            if (Input.GetAxis("Vertical") >= 0.0f)
            {
                Direction = (transform.forward * Input.GetAxis("Vertical")) * Speed * Time.fixedDeltaTime;

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    //走るアニメーション速度変更
                    PlayerAnimator.SetFloat("Speed", 1.5f);
                    SparkParticle.SetActive(true);
                    speedflag = true;//加速時
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    //走るアニメーション速度変更
                    PlayerAnimator.SetFloat("Speed", 0.8f);
                    SparkParticle.SetActive(false);
                    speedflag = false;//通常時
                }

                if (Input.GetAxis("Vertical") != 0f)
                {
                    PlayerAnimator.SetFloat(PlayerActionParameter, 1f);
                }

                if (Input.GetAxis("Horizontal") >= 0f)
                {
                    PlayerAnimator.SetFloat(PlayerActionParameter, 1f);
                }
            }
            else
            {
                //走るアニメーション速度変更
                PlayerAnimator.SetFloat("Speed", 0.8f);
                SparkParticle.SetActive(false);
                speedflag = false;//通常時

                Direction = (transform.forward * Input.GetAxis("Vertical")) * (Speed * 0.6f) * Time.fixedDeltaTime;
                PlayerAnimator.SetFloat("Move", 0.3f);
                PlayerAnimator.SetFloat(PlayerActionParameter, 0.6f);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ChutorialGoodGift")
        {
            TutorialManagerScript.Instance.SetPhaseNumber(1);
            //ChutorialScore.SetScore(1, (int)Score.GIFTSTATUS.giftgood);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "ChutorialBadGift")
        {
            TutorialManagerScript.Instance.SetPhaseNumber(3);
            //ChutorialScore.SetScore(1, (int)Score.GIFTSTATUS.giftbad);
            Destroy(other.gameObject);
        }

        if(other.gameObject.tag == "ChutorialGoodRabbit")
        {
            TutorialManagerScript.Instance.SetPhaseNumber(2);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "ChutorialBadRabbit")
        {
            TutorialManagerScript.Instance.SetPhaseNumber(4);
            Destroy(other.gameObject);
        }
    }
}

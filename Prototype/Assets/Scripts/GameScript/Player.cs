using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public sealed class Player : BaseMonoBehaviour
{
    [SerializeField]
    public Animator PlayerAnimator;
    [SerializeField]
    public CharacterController PlayerController;
    [SerializeField]
    public float Speed;
    private Vector3 velocity;
    private string PlayerActionParameter = "Move";
    private float hit;

    [SerializeField]
    private GameObject GiftArea;//プレイヤーの子要素にギフトを持った時の位置指定してアタッチさせるために必要な変数。

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float rotateSpeed = 120f;

    [SerializeField]
    private int giftMaxNum;

    /// <summary>
    /// 所有ギフトの情報
    /// </summary>
    private float[] giftTime;
    private bool[] giftType;

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

    /// <summary>
    /// 握力時間
    /// </summary>
    [SerializeField] private float holdingTime;

    /// <summary>
    /// 握力経過時間
    /// </summary>
    private float holdingTimeCounter;

    private void awake()
    {
        base.Awake();
    }

    private bool _isGrounded;

    /// <summary>
    /// 地面に接地しているかどうか
    /// </summary>
    public bool IsGrounded { get { return _isGrounded; } }

    // Use this for initialization
    void Start()
    {
        var controller = GetComponent<CharacterController>();

        controller
            .ObserveEveryValueChanged(x => x.isGrounded)
            .ThrottleFrame(2)
            .Subscribe(x => _isGrounded = x);

        goodGiftNum = 0;
        badGiftNum = 0;
        giftTime = new float[giftMaxNum];
        giftType = new bool[giftMaxNum];
        holdingRabbitFlag = false;
        holdingTimeCounter = 0;
        holdingRabbitNumber = -1;
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        // ウサギとの動作
        if (!holdingRabbitFlag)
        {
            CheckRabbit();
        }
        else
        {
            CheckGift();
        }

        PlayerMove();
    }

    private void PlayerMove()
    {
        velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (Input.GetAxis("Vertical") != 0f)
        {
            transform.position += (transform.forward * Input.GetAxis("Vertical")) * speed * Time.fixedDeltaTime;
            PlayerAnimator.SetFloat(PlayerActionParameter, velocity.magnitude);
        }

        if (Input.GetAxis("Horizontal") != 0f)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime, 0);
            PlayerAnimator.SetFloat(PlayerActionParameter, velocity.magnitude);
        }

        if (Input.GetAxis("Vertical") == 0f && Input.GetAxis("Horizontal") == 0f)
        {
            PlayerAnimator.SetFloat(PlayerActionParameter, 0f);
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
    }


    /// <summary>
    /// ウサギを持ち上げられるか確認
    /// </summary>
    private void CheckRabbit()
    {
        for (int i = 0; i < rabbitManager.GetComponent<RabbitManager>().RabbitMaxNum; i++)
        {
            // ボタン入力
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (rabbitManager.GetComponent<RabbitManager>().rabbitManager[i].transform.GetChild(0).gameObject.GetComponent<Circle>().HitPlayerFrag)
                {
                    holdingRabbitFlag = true;
                    holdingTimeCounter = holdingTime;
                    holdingRabbitNumber = i;
                    rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.HOLDING;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckGift()
    {
        if (goodGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Good)
        {
            // 良いギフトがあり良いうさぎなら

            // スコアを反映

            // ウサギを消す
            goodGiftNum = 0;
            holdingRabbitFlag = false;
            rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;

        }
        else if (badGiftNum > 0 && rabbitManager.GetComponent<RabbitManager>().rabbitType[holdingRabbitNumber] == RabbitManager.RabbitType.Bad)
        {
            // 悪いギフトがあり悪いうさぎなら

            // スコアを反映


            // ウサギを消す
            badGiftNum = 0;
            holdingRabbitFlag = false;
            rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;
        }
        else
        {
            // 握力ゲージの処理
            if (Input.GetKey(KeyCode.Space))
            {
                holdingTimeCounter -= Time.deltaTime;
                if (holdingTimeCounter <= 0)
                {
                    holdingRabbitFlag = false;
                    rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;
                }
            }
            else
            {
                holdingRabbitFlag = false;
                rabbitManager.GetComponent<RabbitManager>().rabbitManager[holdingRabbitNumber].GetComponent<RabbitScript>().sCurrentState = RabbitScript.RabbitState.DEAD;
            }

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
}

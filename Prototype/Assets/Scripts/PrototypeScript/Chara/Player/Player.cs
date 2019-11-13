using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
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
    /// ギフトに触れた時状態によって、取得もしくは持ち上げるようにさせる
    /// </summary>
    /// <param name="other">判定の対象物</param>
    private void OnTriggerEnter(Collider other)
    {  
        //普通のギフトの時に当たったら、取得したエフェクト発生させて、ゲージのパラメーターが増加し、ギフト消去させる
        if(other.gameObject.tag == "gift")
        {
            other.gameObject.GetComponent<Gift>().GoodFlag = true;
            // プレイヤーが吸収
            //Destroy(other.gameObject);
        }
        //悪いギフトに当たったら、キャラクタが持ち上げるように位置を変更させ移動できるようにする。
        if(other.gameObject.tag == "Bad gift")
        {
            Debug.Log("あたってる");
            GameStatusManager.Instance.SetLiftGift(true);
            other.gameObject.GetComponent<Gift>().PlayerCarryFlag = true;   // ギフトを運ぶ
            Vector3 m = GiftArea.transform.position;
            other.transform.position = m;
            other.transform.parent = GiftArea.transform;
            other.transform.rotation = Quaternion.identity;
        }
    }
}

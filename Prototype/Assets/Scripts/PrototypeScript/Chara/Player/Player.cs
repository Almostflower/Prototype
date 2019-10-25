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
        PutGift();

        PlayerMove();
    }

    private void PlayerMove()
    {
        if (PlayerController.isGrounded)
        {
            //JoyPadのサイト参考　https://hakonebox.hatenablog.com/entry/2018/04/15/125152
            //入力時の情報　https://qiita.com/RyotaMurohoshi/items/a5cde3c17831adda12db

            velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //Debug.Log(velocity);
            //Debug.Log(velocity.magnitude);

            //止まってる時、動いてる時でifを制御している、　アニメーションで（止まる、歩く、走る）があるが、Moveの数値によって切り替えるように設定している。
            if (velocity.x == 0 && velocity.z == 0)
            {
                PlayerAnimator.SetFloat(PlayerActionParameter, 0f);
            }
            else if (velocity.magnitude > 0.1f)
            {
                PlayerAnimator.SetFloat(PlayerActionParameter, velocity.magnitude);
                transform.LookAt(transform.position + velocity);
            }
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        PlayerController.Move(velocity * Speed * Time.deltaTime);
    }

    public void PutGift()
    {
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
            Destroy(other.gameObject);
        }
        //悪いギフトに当たったら、キャラクタが持ち上げるように位置を変更させ移動できるようにする。
        if(other.gameObject.tag == "Bad gift")
        {
            Debug.Log("あたってる");
            GameStatusManager.Instance.SetLiftGift(true);
            Vector3 m = GiftArea.transform.position;
            other.transform.position = m;
            other.transform.parent = GiftArea.transform;
            other.transform.rotation = Quaternion.identity;
        }
    }
}

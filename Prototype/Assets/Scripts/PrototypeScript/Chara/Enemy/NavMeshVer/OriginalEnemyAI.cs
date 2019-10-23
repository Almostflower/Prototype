using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OriginalEnemyAI : MonoBehaviour
{
    public Transform target;        //ターゲット情報

    private Rigidbody rb;           //リジッドボディ
    private Animator anim;          //管理されたアニメーション情報
    private NavMeshAgent nav;       //NavMesh情報

    #region 移動情報
    /// <param name="WalkSpeed"> 歩く速度 </param>
    /// <param name="RunSpeed"> 走る速度 </param>
    /// もし変更追加などはあればこの構造体内で追加や削除を行うこと
    public float WalkSpeed;
    public float RunSpeed;
    #endregion

    #region 可視情報
    /// <param name="visibleDistance"> 敵から見渡せる距離 </param>
    /// <param name="targetLostLimitTime"> 対象を見失うまでの時間 </param>
    /// <param name="targetFindDistance"> ターゲットを見つけるまでの距離 </param>
    /// <param name="targetDistance"> ターゲットとの距離 </param>
    /// <param name="lostTime"> 見失うまでの時間 </param>
    /// <param name="lineOfSight1"> 目の位置 </param>
    /// <param name="gazeRay"> 目とターゲットを結ぶRay </param>
    /// <param name="visibleLayer"> 見る対象のレイヤー(ターゲットと障害物を含めること) </param>
    public float visibleDistance;
    public float targetLostLimitTime;
    public float targetFindDistance;
    private float targetDistance;
    private float lostTime;
    private Transform lineOfSight1;
    private Ray gazeRay;
    public LayerMask visibleLayer;
    #endregion

    #region 敵情報パラメーター
    /// <param name="lifeMax"> ライフ情報 </param>
    /// <param name="_currentlife"> 現在のライフ </param>
    public int lifeMax;
    private int _currentlife;
    #endregion

    #region 敵のアニメーション管理情報
    /// <param name="idleMaxTime">アイドルアニメーションの最大再生時間</param>
    /// <param name="_idleTime">アイドル再生時の現在時間</parma>
    /// <param name="wanderMaxTime">さまよっている時間</param>
    /// <param name="_wanderTime">さまよっている現在時間</param>
    public float idleMaxTime;
    public float _idleTime;    //初期値0
    public float wanderMaxTime;
    public float _wanderTime; // 初期値0
    #endregion

    #region ステートラベル
    enum EnemyState
    {
        Idle,       //立ち止まっている
        Wander,     //さまよっている
        Chase,      //追っている
        Attack,     //邪魔する
    }
    EnemyState _state = EnemyState.Idle;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        _currentlife = lifeMax;
        lineOfSight1 = GameObject.Find("toko_eye").transform;
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Wander:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
                break;
        }
    }

    void Idle()
    {

    }

    void Wander()
    {

    }

    void Search()
    {

    }

    void TargetFound()
    {

    }

    void Chase()
    {

    }

    void TargetInSight()
    {
        lostTime = 0f;
    }

    void Attack()
    {
        //邪魔するとき
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //anim.SetBool("Attack", true);
            _state = EnemyState.Attack;
            nav.SetDestination(target.position);
            nav.speed = 0f; //navmesh側の速度で移動させない
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

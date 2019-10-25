using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI_2 : BaseMonoBehaviour
{
    public Transform target;        // ターゲットの位置情報

    public int lifeMax;             // ライフ
    int _currentlife;

    Rigidbody rb;
    Animator anim;
    NavMeshAgent nav;

    public float visibleDistance;   // 可視距離
    float targetDistance;           // ターゲットとの距離

    public float sightAngle;        // 視野角

    Transform lineOfSight1;         // 目の位置
    Ray gazeRay1;                   // 目とターゲットを結ぶRay

    public LayerMask visibleLayer;  // 見る対象のLayer（ターゲットと障害物が含まれる）

    public float walkSpeed;         // さまよっているときの速度
    public float runSpeed;          // おいかけているときの速度

    public float targetLostLimitTime;   // ターゲットを見失うまでの時間
    public float targetFindDistance;    // ターゲットを見つける距離（至近距離に近寄ると視野に関係なく見つける）
    float _lostTime = 0f;

    public float idleMaxTime;       // たちどまっている時間
    float _idleTime = 0f;

    public float wanderMaxTime;     // さまよっている時間
    float _wanderTime = 0f;

    enum eState                     // 状態
    {
        Idle,       // 立ち止まっている
        Wander,     // さまよっている
        Chase,      // 追っている
        Attack,     // 攻撃している
        Dead,       // 死んでいる
    }
    eState _state = eState.Idle;

    private void Awake()
    {
        base.Awake();
    }
    // --- 初期化 ----------------------------------------------------------
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        _currentlife = lifeMax;
        lineOfSight1 = GameObject.Find("toko_eye").transform;
    }

    // --- 更新処理 ----------------------------------------------------------
    private void FixedUpdate()
    {
        switch (_state)
        {
            case eState.Idle:
                Idle();
                break;

            case eState.Wander:
                Wander();
                break;

            case eState.Chase:
                Chase();
                break;

            case eState.Attack:
                Attack();
                break;

            case eState.Dead:
                break;
        }

        //if (Input.GetMouseButtonDown(0) && _state != eState.Dead)      // 攻撃を受けたときの処理
        //{
        //    _currentlife--;
        //    Debug.Log("Zombie Life: " + _currentlife);
        //
        //    nav.speed = 0f;
        //
        //    if (_currentlife <= 0)                                      // 死ぬ
        //    {
        //        //anim.SetTrigger("Died");
        //        _state = eState.Dead;
        //        nav.Stop();
        //    }
        //    else                                                        // ダメージを受ける
        //    {
        //        //anim.SetTrigger("Damaged");
        //    }
        //}

        Debug.DrawRay(gazeRay1.origin, gazeRay1.direction * visibleDistance, Color.gray);
    }

    // --- 立ち止まっているときの処理 ----------------------------------------------------------
    void Idle()
    {
        Search(_state);

        _idleTime += Time.deltaTime;
        if (_idleTime > idleMaxTime)                        // 一定時間立ち止まったら、さまよう
        {
            Debug.Log("Wandering");
            //anim.SetTrigger("Walk");
            nav.Resume();
            nav.SetDestination(new Vector3(Random.Range(-14f, 14f), 0f, Random.Range(-14f, 14f))); // ランダムな場所へ向かう
            _state = eState.Wander;
            nav.speed = walkSpeed;
            _idleTime = 0f;
        }
    }

    // --- さまよっているときの処理 ----------------------------------------------------------
    void Wander()
    {
        Search(_state);

        _wanderTime += Time.deltaTime;
        if (_wanderTime > wanderMaxTime || nav.remainingDistance < 0.5f) // 一定時間さまようか行先に着いたら、立ち止まる
        {
            Debug.Log("Idling");
            //anim.SetTrigger("Idle");
            nav.Stop();
            _state = eState.Idle;
            _wanderTime = 0f;
            return;
        }
    }

    // --- ターゲットを探す処理 ----------------------------------------------------------
    void Search(eState state)
    {
        float _angle = Vector3.Angle(target.position - transform.position, lineOfSight1.forward);

        if (_angle <= sightAngle)
        {
            Debug.Log("Target In SightAngle");

            gazeRay1.origin = lineOfSight1.position;
            gazeRay1.direction = target.position - lineOfSight1.position;
            RaycastHit hit;

            if (Physics.Raycast(gazeRay1, out hit, visibleDistance, visibleLayer))
            {
                Debug.DrawRay(gazeRay1.origin, gazeRay1.direction * hit.distance, Color.red);

                if (hit.collider.gameObject.tag != "Obstacle")    // ターゲットとの間に障害物がない
                {
                    if (state == eState.Idle || state == eState.Wander)
                    {
                        TargetFound();
                    }
                    else if (state == eState.Chase)
                    {
                        TargetInSight();
                    }
                    return;
                }
            }

            Debug.DrawRay(gazeRay1.origin, gazeRay1.direction * visibleDistance, Color.gray);
        }

        targetDistance = (transform.position - target.position).magnitude;

        if (targetDistance < targetFindDistance)            // 距離でターゲット発見
        {
            if (state == eState.Idle || state == eState.Wander)
            {
                TargetFound();
            }
            else if (state == eState.Chase)
            {
                TargetInSight();
            }
            return;
        }
    }

    // --- ターゲットを発見したときの処理 ----------------------------------------------------------
    void TargetFound()
    {
        Debug.Log("Target Found");
        //anim.SetTrigger("Run");
        nav.Resume();
        nav.SetDestination(target.position);
        _state = eState.Chase;
        nav.speed = runSpeed;
        _idleTime = 0f;
        _wanderTime = 0f;
    }

    // --- ターゲットを追っているときの処理 ----------------------------------------------------------
    void Chase()                                // 
    {
        nav.SetDestination(target.position);
        Search(_state);

        _lostTime += Time.deltaTime;
        // Debug.Log ("LostTime: " + _lostTime);

        if (_lostTime > targetLostLimitTime)                 // 一定時間視界の外なら、見失う
        {
            Debug.Log("Target Lost");                       // ターゲットロスト
            _state = eState.Idle;
            nav.Stop();
            //anim.SetTrigger("Idle");
            nav.speed = 0f;
            _lostTime = 0f;
        }
    }

    // --- ターゲットが視野に入っているときの処理 ----------------------------------------------------------
    void TargetInSight()
    {
        Debug.Log("Target In Sight");
        _lostTime = 0f;
    }

    // --- 攻撃しているときの処理 ----------------------------------------------------------
    void Attack()
    {
    }

    // --- プレイヤーに接したら攻撃を行う ----------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //anim.SetBool("Attacking", true);
            _state = eState.Attack;
            nav.SetDestination(target.position);
            nav.speed = 0f;
        }
    }

    // --- プレイヤーから離れたら攻撃をやめる ----------------------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //anim.SetBool("Attacking", false);
            _state = eState.Chase;
            nav.speed = runSpeed;
        }
    }
}

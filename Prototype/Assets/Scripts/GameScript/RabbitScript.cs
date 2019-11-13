using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;
using System;

public class RabbitScript : BaseMonoBehaviour
{
    private NavMeshAgent agent;
    private Renderer coloring;
    private GameObject player;
    private float escapeTime = 0;
    private const int layerNumber = 9;
    private int mask;

    [SerializeField]
    private float targetDistance = 5f;

    [SerializeField, Range(0.0f, 360.0f)]
    private float m_searchAngle = 0.0f;

    [SerializeField]
    private float _searchRadius = 40.0f;
    private float m_searchCosTheta = 0.0f;

    public float SearchAngle
    {
        get { return m_searchAngle; }
    }

    public float SearchRadius
    {
        get { return _searchRadius; }
    }

    public enum RabbitState
    {
        ORDINARY,
        TENSION,
        HOLDING,    // 持たれている
        DEAD,       // 死んでいる
    }

    /// <summary>
    /// ウサギの状態
    /// </summary>
    private RabbitState currentState;
    public RabbitState sCurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    private List<Collider> foundList = new List<Collider>();
    private Subject<Unit> touchWithPlayer = new Subject<Unit>();


    public IObservable<Unit> OnTouchWithPlayer
    {
        get { return touchWithPlayer; }
    }

    private void awake()
    {
        base.Awake();
    }

    private void ApplySearchAngle()
    {
        float searchRad = m_searchAngle * 0.5f * Mathf.Deg2Rad;
        m_searchCosTheta = Mathf.Cos(searchRad);
    }

    // シリアライズされた値がインスペクター上で変更されたら呼ばれます。
    private void OnValidate()
    {
        ApplySearchAngle();
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplySearchAngle();

        mask = LayerMask.GetMask(LayerMask.LayerToName(layerNumber));

        //変更予定
        agent = GetComponent<NavMeshAgent>();
        coloring = GetComponent<Renderer>();
        player = GameObject.FindWithTag("Player");

        this.OnCollisionEnterAsObservable()
            .Select(collision => collision.gameObject.tag)
            .Where(tag => tag == "Player")
            .ThrottleFirst(System.TimeSpan.FromSeconds(1))
            .Subscribe(_ => touchWithPlayer.OnNext(Unit.Default));

        ////最初の2回の音と最後の音を分ける
        //enemyCanvas.OnCountChanged
        //    .Where(count => 0 < count && count < 2)
        //    .Subscribe(_ => AudioManager.Instance.PlaySE("SE_TOUCH", 0));
        //
        ////3回タッチされたらゲームオブジェクトを消す
        //enemyCanvas.OnCountChanged
        //    .Where(count => count == 0)
        //    .Subscribe(_ =>
        //    {
        //        AudioManager.Instance.PlaySE("SE_DISAPPEARANCE", 0);
        //        gamemanager.enemyNumverDecrement();
        //        Destroy(gameObject);
        //    });

        Usual();
    }

    void Usual()
    {
        currentState = RabbitState.ORDINARY;
        coloring.material.color = Color.blue;
        agent.angularSpeed = 120;
        Observable.Timer(System.TimeSpan.FromSeconds(1), System.TimeSpan.FromSeconds(3))
                .TakeWhile(_ => currentState == RabbitState.ORDINARY)
                .Where(_ => UnityEngine.Random.Range(1, 100) >= 50)
                .Subscribe(_ =>
                {
                    agent.SetDestination(new Vector3(transform.position.x + UnityEngine.Random.Range(-3, 3), 0f, transform.position.z + UnityEngine.Random.Range(-3, 3)));
                }).AddTo(gameObject);

        //プレイヤーが一定距離に近づいてきたら逃げる
        this.UpdateAsObservable()
            .Select(_ => (transform.position - player.transform.position).sqrMagnitude)
            .Where(distance => distance < targetDistance * targetDistance)
            .First()
            .Subscribe(_ => RunAway()).AddTo(gameObject);
    }

    public void RunAway()
    {
        currentState = RabbitState.TENSION;
        coloring.material.color = Color.red;
        escapeTime = 0;
        agent.angularSpeed = 200;

        //プレイヤーと逆方向を向く
        var diff = (transform.position - player.transform.position).normalized;
        diff.y = 0;
        transform.rotation = Quaternion.FromToRotation(diff, Vector3.up);
        agent.SetDestination(GetNextPosition());

        //目的地に近づいたら次の目的地を検索
        agent.ObserveEveryValueChanged(d => agent.remainingDistance)
            .Where(d => d < 2.0f)
            .Where(_ => currentState == RabbitState.TENSION)
            .Subscribe(_ =>
            {
                var nextposition = GetNextPosition();
                agent.SetDestination(nextposition);
            }).AddTo(gameObject);

        //一定距離離れて一定時間経ったら状態を戻す
        this.UpdateAsObservable()
            .TakeWhile(_ => currentState == RabbitState.TENSION)
            .Select(_ => (transform.position - player.transform.position).sqrMagnitude)
            .Where(distance => distance > targetDistance * targetDistance)
            .Subscribe(distance =>
            {
                escapeTime += Time.deltaTime;
                if (escapeTime >= 10) Usual();
            });
    }

    public Vector3 GetNextPosition()
    {
        if (foundList.Count > 0) foundList.Clear();
        foundList.AddRange(Physics.OverlapSphere(transform.position, _searchRadius, mask));
        if (foundList.Count == 0)
        {
            //近くになかったときは半径40mの中にあるオブジェクトを獲得し、そこからランダムに選ぶ
            foundList.AddRange(Physics.OverlapSphere(transform.position, 40.0f, mask));
            foreach (var obj in foundList) Debug.Log(obj.gameObject.name);
        }
        else
        {
            for (int i = 0; i < foundList.Count; i++)
            {
                var foundData = foundList[i];
                if (!CheckFoundObject(foundData.gameObject))
                    foundList.Remove(foundData);
            }
        }

        return foundList.Count > 0 ? foundList[UnityEngine.Random.Range(0, foundList.Count - 1)].transform.position : Vector3.zero;
    }

    private bool CheckFoundObject(GameObject i_target)
    {
        var myPositionXZ = Vector3.Scale(transform.position, new Vector3(1.0f, 0.0f, 1.0f));
        var targetPositionXZ = Vector3.Scale(i_target.transform.position, new Vector3(1.0f, 0.0f, 1.0f));
        var toTargetFlatDir = (targetPositionXZ - myPositionXZ).normalized;

        //同位置にいるときは範囲内にいるとみなす
        if (toTargetFlatDir.sqrMagnitude <= Mathf.Epsilon) return true;
        return (Vector3.Dot(transform.forward, toTargetFlatDir)) >= m_searchCosTheta;
    }
}

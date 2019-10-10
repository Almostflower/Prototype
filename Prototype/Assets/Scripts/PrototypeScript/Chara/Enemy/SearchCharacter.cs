using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private MoveEnemy moveEnemy;

    // Start is called before the first frame update
    void Start()
    {
        moveEnemy = GetComponentInParent<MoveEnemy>();    
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("キャラクターサーチ");
    }

    //壁などの判定もしくはナヴメッシュを使ってみる。
    
    /// <summary>
    /// プレイヤータグ検出用判定
    /// </summary>
    /// <param name="other">判定検出</param>
    private void OnTriggerStay(Collider other)
    {
        //プレイヤーキャラクターを検出
        if(other.tag == "Player")
        {
            //敵キャラクターの状態検出
            MoveEnemy.EnemyState state = moveEnemy.GetState();
            //敵キャラクターが追いかけていない状態なら追いかける。
            if(state != MoveEnemy.EnemyState.Chase)
            {
                Debug.Log("プレイヤー発見");
                moveEnemy.SetState(MoveEnemy.EnemyState.Chase, other.transform);
            }
        }
    }

    /// <summary>
    /// プレイヤーが検出範囲からはずれた際に待機状態へ移す
    /// </summary>
    /// <param name="other">判定検出</param>
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player Lost");

            moveEnemy.SetState(MoveEnemy.EnemyState.Wait);
        }
    }
}

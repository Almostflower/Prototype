using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDolly : MonoBehaviour
{
    // マウスホイールの回転値を格納する変数
    private float scroll = 0.0f;
    // カメラ移動の速度
    public float speed = 1f;
    // ドリーインの限界判定フラグ
    private bool dollyInLimit = false;
    //ドリーアウトの限界判定フラグ
    private bool dollyOutLimit = false;
    // ドリーインの限界判定コライダーを格納する変数
    public Collider dollyInLimitCol;
    // ドリーアウトの限界判定コライダーを格納する変数
    public Collider dollyOutLimitCol;

    // ゲーム実行中の繰り返し処理
    void Update()
    {
        // マウスホイールの回転値を変数 scroll に渡す
        scroll -= Time.deltaTime;

        //ドリーインの限界でホイールスクロール値が負の場合
        if (dollyInLimit && scroll > 0)
        {
            // カメラの前後移動処理を行わない（0を加算して移動停止）
            transform.position += transform.forward * 0;
        }
        //ドリーアウトの限界でホイールスクロール値が正の場合
        else if (dollyOutLimit && scroll < 0)
        {
            // カメラの前後移動処理を行わない（0を加算して移動停止）
            transform.position += transform.forward * 0;
        }
        //上記以外の場合（ドリーイン、ドリーアウトの限界でない）
        else
        {
            // カメラの前後移動処理
            //（カメラが向いている方向 forward に変数 scroll と speed を乗算して加算する）
            Camera.main.transform.position += transform.forward * scroll * speed;
        }
    }

    // コライダーとの衝突した場合の判定処理
    void OnTriggerEnter(Collider other)
    {
        // 衝突したコライダーが dollyInLimitCol の場合
        if (other == dollyInLimitCol)
        {
            // ドリーインの限界判定フラグを有効
            dollyInLimit = true;
        }
        // 衝突したコライダーが dollyOutLimitCol の場合
        else if (other == dollyOutLimitCol)
        {
            // ドリーアウトの限界判定フラグを有効
            dollyOutLimit = true;
        }
    }

    // コライダーとの衝突が解除した場合の処理
    void OnTriggerExit(Collider other)
    {
        // ドリーインの限界判定フラグを無効
        dollyInLimit = false;
        // ドリーアウトの限界判定フラグを無効
        dollyOutLimit = false;
    }
}

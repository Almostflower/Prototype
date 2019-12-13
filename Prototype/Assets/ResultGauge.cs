using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultGauge : BaseMonoBehaviour
{
    ///////////////////////////////////////////////////////////////////
    /// プロトを見て変える
    /// <summary>
    /// 1本のゲージで見せるモード
    /// </summary>
    private bool debug_one_time_ = false;
    private float dustlimittime_ = 0;
    ///////////////////////////////////////////////////////////////////

    /// <summary>
    /// ギフト専用かのフラグ
    /// </summary>
    [SerializeField]
    private bool gift_type = false;

    /// <summary>
    /// スライダー
    /// </summary>
    [SerializeField]
    private Slider slider_;

    /// <summary>
    /// スライダーの中の背景画像
    /// </summary>
    [SerializeField]
    private Image background_;

    /// <summary>
    /// スライダーの中の動かす背景
    /// </summary>
    [SerializeField]
    private Image fill_;

    /// <summary>
    /// メインカメラのオブジェクト
    /// </summary>
    private Camera main_camera_;

    /// <summary>
    /// 減算する値
    /// </summary>
    private float gauge_value_;
    public float GaugeValue
    {
        set { gauge_value_ = value; }
    }

    /// <summary>
    /// 一回きりのフラグ
    /// </summary>
    private bool once_ = false;

    /// <summary>
    /// キャンバスのオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject canvas_;

    /// <summary>
    /// BaseMonoBehaviourの初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        once_ = false;

        // ギフト専用初期化
        if (gift_type)
        {
            //変更
            //z座標は　slider_transform.position.zに指定してしまうと、座標が元の位置からずれてしまうので0.0fに変更しました。
            slider_.transform.localPosition = new Vector3(0.0f, slider_.transform.position.y, 0.0f);
            slider_.transform.Rotate(new Vector3(0, 1, 0), 180);
        }

        // 最大秒数をスライダーのマックス値に代入
        //slider_.maxValue = gauge_value_;
    }

    public override void UpdateNormal()
    {
        ///////////////////////////////////////////////////////////////////
        /// プロトを見て変える
        /// <summary>
        if (debug_one_time_)
        {
            background_.color = new Color32(0, 0, 0, 255);
            if (gauge_value_ < dustlimittime_)
            {
                fill_.color = new Color32(255, 255, 0, 255);
            }
            else
            {
                fill_.color = new Color32(0, 255, 0, 255);
            }
        }
        ///////////////////////////////////////////////////////////////////

        // ギフト専用カメラ
        if (gift_type)
        {
            Vector3 p = Camera.main.transform.position;
            p.y = transform.position.y;
            // 常にカメラに向く
            canvas_.transform.LookAt(p);
        }

        // ゲージに値を設定
        slider_.value += 0.005f;

        // 背景のゲージ
        //background_.fillAmount = gauge_value_ / slider_.maxValue + (1 - gauge_value_ / slider_.maxValue) * 0.1f;
        background_.fillAmount += 0.005f;

        // スライダーの値が0以下になったら最大値に戻す
        if (slider_.value <= 0 && once_)
        {
            slider_.value = slider_.maxValue;
            once_ = false;
        }
    }

    /// <summary>
    /// ギフトの値をスライダーマックス値にセット関数
    /// </summary>
    /// /// <param name="seconds"></param>
    public void SetGiftValue(float seconds)
    {
        slider_.maxValue = seconds;
        fill_.color = new Color32(255, 255, 0, 255);
        slider_.value = seconds;
        background_.color = new Color32(0, 0, 0, 255);
    }

    /// <summary>
    /// 最初のスライダーマックス値のセット関数
    /// </summary>
    /// <param name="val"></param>
    /// <param name="dusttime"></param>
    public void SetMaxValue(float val, float dusttime = 0, bool doubletime = false, bool giftonce = false)
    {
        //slider_.maxValue = val;
        //slider_.value = slider_.maxValue;
        once_ = giftonce;

        ///////////////////////////////////////////////////
        /// ここもプロトを見て変える
        dustlimittime_ = dusttime;
        debug_one_time_ = doubletime;
    }
}

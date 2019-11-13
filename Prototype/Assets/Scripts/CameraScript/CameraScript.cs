using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform Target;
    public float DistanceToPlayerM = 2f;    // カメラとプレイヤーとの距離[m]
    public float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    public float HeightM = 1.2f;            // 注視点の高さ[m]
    public float RotationSensitivity = 100f;// 感度

    void Start()
    {
        if (Target == null)
        {
            Debug.LogError("ターゲットが設定されていない");
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        //var rotX = Input.GetAxis("Horizontal") * Time.deltaTime * RotationSensitivity;
        //var rotY = Input.GetAxis("Vertical") * Time.deltaTime * RotationSensitivity;

        var lookAt = Target.position + Vector3.up * HeightM;

        //// 回転
        //transform.RotateAround(lookAt, Vector3.up, rotX);
        //// カメラがプレイヤーの真上や真下にあるときにそれ以上回転させないようにする
        //if (transform.forward.y > 0.9f && rotY < 0)
        //{
        //    rotY = 0;
        //
        //    if (SlideDistanceM < 0.0f)
        //    {
        //        SlideDistanceM += 0.01f;
        //    }
        //}
        //if (transform.forward.y < -0.9f && rotY > 0)
        //{
        //    rotY = 0;
        //
        //    if (SlideDistanceM > 0.0f)
        //    {
        //        SlideDistanceM -= 0.01f;
        //    }
        //}

        //transform.RotateAround(lookAt, transform.right, rotY);

        // カメラとプレイヤーとの間の距離を調整
        transform.position = lookAt - transform.forward * DistanceToPlayerM;

        // 注視点の設定
        transform.LookAt(lookAt);

        // カメラを横にずらして中央を開ける
        transform.position = transform.position + transform.right * SlideDistanceM;
    }

    //*カメラスクリプト方法２
    
    //[SerializeField]
    //private Transform character;    //キャラクターをInspectorウィンドウから選択してください
    //[SerializeField]
    //private Transform pivot;    //キャラクターの中心にある空のオブジェクトを選択してください

    //void Start()
    //{
    //    //エラーが起きないようにNullだった場合、それぞれ設定
    //    if (character == null)
    //        character = transform.parent;
    //    if (pivot == null)
    //        pivot = transform;
    //}
    ////カメラ上下移動の最大、最小角度です。Inspectorウィンドウから設定してください
    //[Range(-0.999f, -0.5f)]
    //public float maxYAngle = -0.5f;
    //[Range(0.5f, 0.999f)]
    //public float minYAngle = 0.5f;
    //// Update is called once per frame
    //void Update()
    //{
    //    //マウスのX,Y軸がどれほど移動したかを取得します
    //    float X_Rotation = Input.GetAxis("Mouse X");
    //    float Y_Rotation = Input.GetAxis("Mouse Y");
    //    //Y軸を更新します（キャラクターを回転）取得したX軸の変更をキャラクターのY軸に反映します
    //    character.transform.Rotate(0, X_Rotation, 0);

    //    //次はY軸の設定です。
    //    float nowAngle = pivot.transform.localRotation.x;
    //    //最大値、または最小値を超えた場合、カメラをそれ以上動かない用にしています。
    //    //キャラクターの中身が見えたり、カメラが一回転しないようにするのを防ぎます。
    //    if (-Y_Rotation != 0)
    //    {
    //        if (0 < Y_Rotation)
    //        {
    //            if (minYAngle <= nowAngle)
    //            {
    //                pivot.transform.Rotate(-Y_Rotation, 0, 0);
    //            }
    //        }
    //        else
    //        {
    //            if (nowAngle <= maxYAngle)
    //            {
    //                pivot.transform.Rotate(-Y_Rotation, 0, 0);
    //            }
    //        }
    //    }
    //}
}
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //public Transform Target;
    //public float DistanceToPlayerM = 2f;    // カメラとプレイヤーとの距離[m]
    //public float SlideDistanceM = 0f;       // カメラを横にスライドさせる；プラスの時右へ，マイナスの時左へ[m]
    //public float HeightM = 1.2f;            // 注視点の高さ[m]
    //public float RotationSensitivity = 100f;// 感度
    
    public GameObject rotateObject;
    Vector3 defDis = new Vector3(0, 2, -7.5f);
    private Vector3 preWorldlPos;

    // Use this for initialization
    void Start()
    {
        transform.localPosition = defDis;
        preWorldlPos = this.transform.position;
    }


    //void Start()
    //{
    //    if (Target == null)
    //    {
    //        Debug.LogError("ターゲットが設定されていない");
    //        Application.Quit();
    //    }
    //}

    void FixedUpdate()
    {
        Vector3 localPos = this.transform.localPosition;

        /*カメラを上下に動かしたときの目標カメラ位置補正*/
        float angleX = rotateObject.transform.localEulerAngles.x;
        if (rotateObject.transform.localEulerAngles.x > 180)
        {
            //上を向いたとき
            angleX = Mathf.Abs(angleX - 360);
            localPos.y = defDis.y - defDis.y * ((angleX) / 70);
            localPos.z = defDis.z - defDis.z * ((angleX) / 70);
        }
        else
        {
            //下を向いたとき
            localPos.z = defDis.z - defDis.z * ((angleX) / 70);
            localPos.y = defDis.y + Mathf.Abs(localPos.z - defDis.z);
        }

        /*ローカルX軸のカメラワークに余裕を持たせる*/
        float angleY = rotateObject.transform.eulerAngles.y;
        //ローカルX軸い前回からどれくらい動いたかを求める
        Vector3 diffVec = Quaternion.Euler(0f, -angleY, 0f) * (this.transform.position - preWorldlPos);
        localPos.x = localPos.x - diffVec.x;
        if (localPos.x >= 0.5f)
        {
            localPos.x = 0.5f;
        }
        else if (localPos.x <= -0.5f)
        {
            localPos.x = -0.5f;
        }

        this.transform.localPosition = localPos;

        preWorldlPos = this.transform.position;

        //var lookAt = Target.position + Vector3.up * HeightM;
        //
        //// カメラとプレイヤーとの間の距離を調整
        //transform.position = lookAt - (transform.forward + direction) * DistanceToPlayerM;
        //
        //// 注視点の設定
        //transform.LookAt(lookAt);
        //
        //// カメラを横にずらして中央を開ける
        //transform.position = transform.position + transform.right * SlideDistanceM;
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
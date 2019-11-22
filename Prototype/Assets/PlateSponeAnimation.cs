using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSponeAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject plate;

    [SerializeField, Tooltip("スケールの最大値")]
    private Vector3 objscale;

    [SerializeField, Tooltip("エフェクトスポーンフラグ")]
    private bool effectflag;

    [SerializeField, Tooltip("スケール増加値")]
    private float addscale;

    [SerializeField, Tooltip("回転方向")]
    private Vector3 rotateAxis;

    [SerializeField, Tooltip("回転を速度で判定するか、時間で判定するか")]
    private bool rotatemode;

    [SerializeField, Tooltip("回転速度")]
    private float rotatespeed;

    [SerializeField, Tooltip("回転速度の減速")]
    private float rotateminus;

    [Tooltip("回転時間")]
    private float rotatetime;

    [SerializeField, Tooltip("回転停止させる時間")]
    private float rotatestoptime;

    [Tooltip("回転止めるフラグ")]
    private bool RotateStopFlag;

    [SerializeField, Tooltip("初期化用のRotation数値")]
    private float DefaultRotateAngle;

    [SerializeField, Tooltip("RotationAxis")]
    private Vector3 DefaultRotateAxis;

    [SerializeField, Tooltip("CreateObjectParticle")]
    private ParticleSystem ps;

    [Tooltip("InstantiateParticle")]
    private ParticleSystem InstanParticle;

    [SerializeField, Tooltip("Particle Angle")]
    private float PsAngle;

    [SerializeField, Tooltip("Particle Angle Axis")]
    private Vector3 PsAngleAxis;

    [SerializeField, Tooltip("Particle Destroy Time")]
    private float ParticleDestroyTime;

    [Tooltip("NowTime")]
    private float PsNowTime;
    void Start()
    {
        plate.gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        rotatetime = 0.0f;
        RotateStopFlag = false;
        PsNowTime = 0.0f;
        if (effectflag)
        {
            InstanParticle = Instantiate(ps, plate.gameObject.transform.position, Quaternion.AngleAxis(PsAngle, PsAngleAxis));
            InstanParticle.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(effectflag)
        {
            PsNowTime += Time.deltaTime;
        }

        if (rotatemode)
        {
            if (rotatespeed > 0)
            {
                rotatespeed -= rotateminus;
            }
        }
        else
        {
            if(rotatetime > rotatestoptime)
            {
                plate.gameObject.transform.rotation = Quaternion.AngleAxis(DefaultRotateAngle, DefaultRotateAxis);

                RotateStopFlag = true;
            }
            rotatetime += Time.deltaTime;
        }

        if(plate.gameObject.transform.localScale.x < objscale.x)
        {
            plate.gameObject.transform.localScale += new Vector3(addscale, 0.0f, 0.0f);
        }
        if(plate.gameObject.transform.localScale.y < objscale.y)
        {
            plate.gameObject.transform.localScale += new Vector3(0.0f, addscale, 0.0f);
        }
        if(plate.gameObject.transform.localScale.z < objscale.z)
        {
            plate.gameObject.transform.localScale += new Vector3(0.0f, 0.0f, addscale);
        }

        if(rotatemode)
        {
            plate.gameObject.transform.Rotate(rotateAxis, rotatespeed);
        }
        else
        {
            if(!RotateStopFlag)
            {
                plate.gameObject.transform.Rotate(rotateAxis, rotatespeed);
            }
        }

        if(effectflag)
        {
            //if (PsNowTime > ParticleDestroyTime) ;
            //{
            //    InstanParticle.Stop();
            //}
            Destroy(InstanParticle, ParticleDestroyTime);
        }
    }
}

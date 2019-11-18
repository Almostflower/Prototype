using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootParticle : BaseMonoBehaviour
{
    public GameObject particle;//Particleを宣言
    private float foottime;
    // Use this for initialization
    protected void Awake()
    {
        base.Awake();
    }

    public override void UpdateNormal()
    {
        this.foottime += Time.deltaTime;
        if (this.foottime > 0.35f)
        {
            this.foottime = 0;
            Instantiate(particle, transform.position, Quaternion.AngleAxis(90, new Vector3(1, 0, 0)));
        }
    }

}

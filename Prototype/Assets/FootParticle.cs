using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootParticle : MonoBehaviour
{
    public GameObject particle;//Particleを宣言
    private float foottime;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.foottime += Time.deltaTime;
        if (this.foottime > 0.35f)
        {
            this.foottime = 0;
            Instantiate(particle, transform.position, Quaternion.AngleAxis(90,new Vector3(1,0,0)));
        }
    }
}

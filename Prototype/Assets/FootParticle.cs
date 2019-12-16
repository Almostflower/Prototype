using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootParticle : BaseMonoBehaviour
{
    [SerializeField]
    private GameObject particle;//Particleを宣言
    private GameObject[] particles;
    [SerializeField]
    private int createMax;
    private float foottime = 0.0f;
    [SerializeField]
    GameObject footpos;
    // Use this for initialization
    protected void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        particles = new GameObject[createMax];
        for(int i = 0; i < createMax; i++)
        {
            particles[i] = Instantiate(particle, transform.position, Quaternion.AngleAxis(90, new Vector3(1, 0, 0)));
        }
    }
    public override void UpdateNormal()
    {
        this.foottime += Time.deltaTime;
        if (this.foottime > 0.35f)
        {
            for (int i = 0; i < createMax; i++)
            {
                particles[i].gameObject.transform.position = footpos.transform.position;
            }
            this.foottime = 0.0f;
        }
    }

}

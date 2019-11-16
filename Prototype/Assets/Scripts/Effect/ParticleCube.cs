using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCube : BaseMonoBehaviour
{
    private float speed;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.5f;
        this.transform.localScale = new Vector3(0.05f, Random.Range(0.05f, 0.1f), 0.05f);
        //this.GetComponent<Material>().color = Color.Lerp(Color.blue, Color.green, Random.Range(1.0f, 10.0f));
        this.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        Vector3 force;
        force = this.transform.up * speed;
        this.GetComponent<Rigidbody>().AddForce(force);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCube : BaseMonoBehaviour
{
    [SerializeField, Range(0.05f,0.1f),Tooltip("Cubeの大きさ")] private float cubeSize;
    [SerializeField, Range(0.1f, 1.0f),Tooltip("上昇スピード")] private float upSpeed;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("赤")] private float red;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("緑")] private float green;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("青")] private float blue;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale = new Vector3(cubeSize, Random.Range(cubeSize, cubeSize * 10), cubeSize);
        this.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue));
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        Vector3 force;
        force = this.transform.up * upSpeed;
        this.GetComponent<Rigidbody>().AddForce(force);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCube : BaseMonoBehaviour
{
    /// <summary>
    /// ふり幅
    /// </summary>
    [SerializeField, Tooltip("ふり幅最小")]private int swingWidthMin;
    [SerializeField, Tooltip("ふり幅最大")] private int swingWidthMax;
    private int swingWidth;


    /// <summary>
    /// 周期（何秒で一周するか）
    /// </summary>
    [SerializeField, Tooltip("周期最小")] private int fMin;
    [SerializeField, Tooltip("周期最大")] private int fMax;
    private float f;



    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        swingWidth = Random.RandomRange(swingWidthMin, swingWidthMax);
        f = (float)(Random.RandomRange(fMin, fMax));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        float sin = swingWidth * Mathf.Sin(2 * Mathf.PI * (float)(1 / f) * Time.time);

        this.transform.position = new Vector3(pos.x, sin, pos.z);
    }
}

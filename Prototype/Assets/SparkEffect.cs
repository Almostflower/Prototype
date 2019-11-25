using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEffect : BaseMonoBehaviour
{
    [SerializeField]
    private GameObject ps;
    
    // Start is called before the first frame update
    private void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //Destroy(ps, 3);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

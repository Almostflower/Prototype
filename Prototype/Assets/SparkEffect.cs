using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject ps;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(ps, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

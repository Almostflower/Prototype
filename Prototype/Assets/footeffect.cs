using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footeffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(ps, 3.0f);
    }
}

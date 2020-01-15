using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playershadow : MonoBehaviour
{
    [SerializeField]
    private GameObject shadowobj;
    [SerializeField]
    private float ypos;
    // Start is called before the first frame update
    void Start()
    {
        shadowobj.transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        shadowobj.transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
    }
}

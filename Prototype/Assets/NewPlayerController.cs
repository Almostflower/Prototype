using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float rotateSpeed = 120f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private Vector3 velocity;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") != 0f)
        {
            transform.position += (transform.forward * Input.GetAxis("Vertical")) * speed * Time.fixedDeltaTime;
        }

        if(Input.GetAxis("Horizontal") != 0f)
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed * Time.fixedDeltaTime, 0);
        }

    }
}

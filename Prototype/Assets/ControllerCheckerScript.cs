using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public class ControllerCheckerScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    //処理を書く
                    Debug.Log(code);
                    break;
                }
            }
        }

        //transform.position = new Vector3(
        //    transform.position.x + Input.GetAxis("Horizontal"),
        //    0,
        //    transform.position.z + Input.GetAxis("Vertical")
        //);

        //Debug.Log(Input.GetAxisRaw("Horizontal2"));
        Debug.Log(Input.GetAxisRaw("Vertical"));
    }
}

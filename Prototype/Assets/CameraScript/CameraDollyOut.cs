using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDollyOut : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject MovePoint;

    private bool dollyflag;

    void Start() {
       dollyflag = false;
    }

    void Update () {

        // Simple control to allow the camera to be moved in and out using the up/down arrows.
        transform.position = Vector3.Lerp(transform.position, MovePoint.transform.position, Time.deltaTime);
        transform.LookAt(target.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform PositionTarget;
    public Vector3 offset;

    void Start()
    {
        offset = transform.position - PositionTarget.position;
    }
    void Update()
    {
        transform.position = PositionTarget.position + offset;
    }
}

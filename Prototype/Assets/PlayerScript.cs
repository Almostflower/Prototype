using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    CharacterController controller;
    Vector3 moveDirection;

    float fSpeed = 3.0f;

    void Start()
    {
        controller = GetComponent("CharacterController") as CharacterController;
    }

    void Update()
    {
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
        moveDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
        moveDirection *= fSpeed;

        // 移動
        controller.Move(moveDirection * Time.deltaTime);
    }
}

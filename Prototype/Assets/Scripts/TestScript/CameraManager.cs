using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;

    public float offsetx;
    public float offsety;
    public float offsetz;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position;

        transform.position = new Vector3(pos.x + offsetx, pos.y + offsety, pos.z + offsetz);

        transform.LookAt(player.transform);

    }
}

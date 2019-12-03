using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.transform.GetChild(0).GetComponent<Circle>().HitPlayerFrag)
            {
                Vector3 direction = this.transform.GetChild(1).transform.position -
                                        this.transform.GetChild(0).transform.position;

                player.GetComponent<Player>().SetDirection(direction);
            }

            if (this.transform.GetChild(1).GetComponent<Circle>().HitPlayerFrag)
            {
                Vector3 direction = this.transform.GetChild(0).transform.position -
                                        this.transform.GetChild(1).transform.position;
                player.GetComponent<Player>().SetDirection(direction);
            }
        }
        
    }
}

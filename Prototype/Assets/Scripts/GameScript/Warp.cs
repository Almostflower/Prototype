using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : BaseMonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform warp1, warp2;
    private void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.transform.GetChild(0).GetComponent<Circle>().HitPlayerFrag)
            {
                Vector3 direction = warp2.position -
                                        this.transform.GetChild(0).transform.position;

//                player.GetComponent<TutorialPlayer>().SetDirection(direction);
                player.GetComponent<Player>().SetDirection(direction);
            }

            if (this.transform.GetChild(1).GetComponent<Circle>().HitPlayerFrag)
            {
                Vector3 direction = warp1.position -
                                        this.transform.GetChild(1).transform.position;
                //player.GetComponent<TutorialPlayer>().SetDirection(direction);
                                player.GetComponent<Player>().SetDirection(direction);
            }
        }
        
    }
}

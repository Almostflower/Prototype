﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    /// <summary>
    /// プレイヤーとのあたり判定
    /// </summary>
    [SerializeField] private bool hitPlayerFrag;
    public bool HitPlayerFrag
    {
        get { return hitPlayerFrag; }
    }

    // Start is called before the first frame update
    void Start()
    {
        hitPlayerFrag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ウサギを捕まえる範囲に入ったら
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hitPlayerFrag = true;
        }
    }


    /// <summary>
    /// ウサギの捕まえる範囲から離れたら
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hitPlayerFrag = false;
        }
    }
}

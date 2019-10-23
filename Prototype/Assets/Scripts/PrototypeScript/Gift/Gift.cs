﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : BaseMonoBehaviour
{
    /// <summary>
    /// ギフトの寿命
    /// </summary>
    [SerializeField]private float time;

    /// <summary>
    /// 経過タイム
    /// </summary>
    private float progressTime;

    /// <summary>
    /// ギフトの生存確認
    /// </summary>
    private bool isDead;
    public bool IsDead
    {
        get { return isDead; }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ギフトの初期化
    /// </summary>
    private void Start()
    {
        progressTime = 0;
        isDead = false;
    }

    /// <summary>
    /// ギフトの更新
    /// </summary>
    public override void UpdateNormal()
    {
        if (isDead) { return; }

        progressTime += Time.deltaTime;

        if(progressTime >= 1)
        {
            time--;
            progressTime = 0;
        }

        if(time <= 0)
        {
            isDead = true;
        }

        Debug.Log("ギフトタイム" + time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : SingletonMonoBehaviour<GameStatusManager>
{
    private bool LiftGift;//プレイヤーがギフトを持っているか確認するフラグ

    private void Start()
    {
        LiftGift = false;
    }
    public void SetLiftGift(bool flag)
    {
        LiftGift = flag;
    }

    public bool GetLiftGift()
    {
        return LiftGift;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : SingletonMonoBehaviour<GameStatusManager>
{
    private bool LiftGift;//プレイヤーがギフトを持っているか確認するフラグ

    public enum GameState
    {
        GOOD,
        NORMAL,
        BAD,
        NONE
    }

    GameState state;
    private void Start()
    {
        LiftGift = false;
        state = GameState.NORMAL;
    }
    public void SetLiftGift(bool flag)
    {
        LiftGift = flag;
    }

    public bool GetLiftGift()
    {
        return LiftGift;
    }

    public GameState GetGameState()
    {
        return state;
    }

    public void SetGameState(GameState setstate)
    {
        state = setstate;
    }
}

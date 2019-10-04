using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatusManager : SingletonMonoBehaviour<StatusManager>
{
    [SerializeField]
    public float StageWidth;
    [SerializeField]
    public int GiftMaximum;
    [SerializeField]
    public int StageLevel;
    [SerializeField]
    public int StageMaximum;
    [SerializeField]
    public float[] StageLimitTimer;
    
    private int GiftNowNum;
    private bool IsPossession;
    public int GetGiftMax()
    {
        return GiftMaximum;
    }

    public int GetGiftNum()
    {
        return GiftNowNum;
    }

    public float GetStageWidth()
    {
        return StageWidth;
    }

    public void SetStageWidthSize(float width)
    {
        StageWidth = width;
    }

    public bool GetNowPossession()
    {
        return IsPossession;
    }

    public void SetPossession(bool Possession)
    {
        IsPossession = Possession;
    }
    
    public int GetStageLevel()
    {
        return StageLevel;
    }

    public void SetStageLevel(int stage)
    {
        StageLevel = stage;
    }

    public int GetStageMaximum()
    {
        return StageMaximum;
    }

    public void SetStageMaximum(int Maximum)
    {
        StageMaximum = Maximum;
    }

    public float GetStageLimitTimer(int StageLevel)
    {
        return StageLimitTimer[StageLevel];
    }

    public void SetStageLimitTimer(int StageLevel, float LimitTimer)
    {
        StageLimitTimer[StageLevel] = LimitTimer;
    }
}

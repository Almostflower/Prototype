using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StatusManager : SingletonMonoBehaviour<StatusManager>
{
    [SerializeField]
    private float StageWidth;
    [SerializeField]
    private int GiftMaximum;
    [SerializeField]
    private int StageLevel;
    [SerializeField]
    private int StageMaximum;
    [SerializeField]
    private float[] StageLimitTimer;
    
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerScript : SingletonMonoBehaviour<TutorialManagerScript>
{
    private int Phase;
    private bool PhaseChange;
    private bool TimeChecker;
    private void Start()
    {
        Phase = 0;
        PhaseChange = false;
        TimeChecker = true;
    }

    public int GetPhaseNumber()
    {
        Debug.Log(Phase);
        return Phase;
    }

    public void SetPhaseNumber(int phase_num)
    {
        Phase = phase_num;
    }

    public bool GetPhaseCheck()
    {
        return PhaseChange;
    }

    public void SetPhaseCheck(bool phasechack)
    {
        PhaseChange = phasechack;
    }

    public bool GetTimeCheckFlag()
    {
        return TimeChecker;
    }

    public void SetTimeCheckFlag(bool TimeFlag)
    {
        TimeChecker = TimeFlag;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManagerScript : SingletonMonoBehaviour<TutorialManagerScript>
{
    private int Phase;
    
    private void Start()
    {
        Phase = 0;
    }

    public int GetPhase()
    {
        return Phase;
    }

    public void SetPhase(int phase_num)
    {
        Phase = phase_num;
    }
}

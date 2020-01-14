using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : BaseMonoBehaviour
{
    private int ArrowMoveFlag;
    private float movetime;

    private void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        ArrowMoveFlag = -1;
    }

    // Update is called once per frame
    public override void UpdateNormal()
    {
        movetime += Time.deltaTime;

        if(movetime >= 0.4f)
        {
            movetime = 0.0f;
            ArrowMoveFlag *= -1;
        }


        if (ArrowMoveFlag == -1)
        {
            this.transform.position += new Vector3(0.0f, 0.25f, 0.0f);
        }
        else
        {
            transform.position += new Vector3(0.0f, -0.25f, 0.0f);
        }
    }
}

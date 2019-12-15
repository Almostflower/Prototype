using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject select1;
    [SerializeField]
    private GameObject select2;

    public float target_rotate = 90;

    // Use this for initialization
    void Start()
    {

    }
    bool acedeselect = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Vertical") >= 1.0f && !acedeselect)
        {
            acedeselect = true;
            SceneStatusManager.Instance.TitleStart = 1;
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
        }
        if(Input.GetAxisRaw("Vertical") <= -1.0f && acedeselect)
        {
            acedeselect = false;
            SceneStatusManager.Instance.TitleStart = -1;
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundManager.SingletonInstance.PlaySE(SoundManager.SELabel.Catch_SE);
            SceneStatusManager.Instance.TitleStart *= -1;
        }

        if(SceneStatusManager.Instance.TitleStart == -1)
        {
            select1.SetActive(true);
            select2.SetActive(false);
            RotateSelect(select1, 0,-2.5f);
            RotateSelect(select2, 90, 2.5f);
        }
        else
        {
            select1.SetActive(false);
            select2.SetActive(true);
            RotateSelect(select1, 90, 2.5f);
            RotateSelect(select2,0,-2.5f);
        }
    }

    void RotateSelect(GameObject selectobj, float targetrotate, float rotation)
    {
        var target = Quaternion.Euler(new Vector3(0, targetrotate, 0));

        var now_rot = selectobj.transform.rotation;
        if (Quaternion.Angle(now_rot, target) <= 1)
        {
            selectobj.transform.rotation = target;
        }
        else
        {
            selectobj.transform.Rotate(new Vector3(0, rotation, 0));
        }
    }
}

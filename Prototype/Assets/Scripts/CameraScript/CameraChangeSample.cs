using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeSample : MonoBehaviour
{
    [SerializeField]
    private GameObject MainCamera;
    [SerializeField]
    private GameObject SubCamera1;
    [SerializeField]
    private GameObject SubCamera2;
    [SerializeField]
    private GameObject SubCamera3;
    [SerializeField]
    private int SubCameraNum;

    private int CameraType;
    private float NowTime = 0.0f;
    [SerializeField]
    private float ChangeTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        CameraType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(CameraType >= 4)
        {
            CameraType = 0;
        }
        NowTime += Time.deltaTime;

        switch (CameraType)
        {
            case 0:
                MainCamera.SetActive(true);
                SubCamera1.SetActive(false);
                SubCamera2.SetActive(false);
                SubCamera3.SetActive(false);
                break;
            case 1:
                MainCamera.SetActive(false);
                SubCamera1.SetActive(true);
                SubCamera2.SetActive(false);
                SubCamera3.SetActive(false);
                break;
            case 2:
                MainCamera.SetActive(false) ;
                SubCamera1.SetActive(false);
                SubCamera2.SetActive(true);
                SubCamera3.SetActive(false);
                break;
            case 3:
                MainCamera.SetActive(false);
                SubCamera1.SetActive(false);
                SubCamera2.SetActive(false);
                SubCamera3.SetActive(true);
                break;
        }

        if(NowTime > ChangeTime)
        {
            NowTime = 0.0f;
            CameraType += 1;
        }
    }
}

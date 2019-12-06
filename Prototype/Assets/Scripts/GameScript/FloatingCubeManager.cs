using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCubeManager : MonoBehaviour
{
    /// <summary>
    /// キューブのプレファブ
    /// </summary>
    [SerializeField] private GameObject cube;

    /// <summary>
    /// キューブの生成数
    /// </summary>
    [SerializeField] private int cubeNum;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < cubeNum; i++)
        {
            float x = Random.Range(-60, 60);
            float y = Random.Range(-5, 5);
            float z = Random.Range(-60, 60);
            GameObject floatingCube = Instantiate(cube, new Vector3(x,y,z), Quaternion.identity);
            floatingCube.transform.parent = this.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

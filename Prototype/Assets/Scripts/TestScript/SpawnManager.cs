using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] int amountToSpawn = 10;
    [SerializeField] public GameObject cubeprefab;

    int spawnAmount;


    void Start()
    {
        while(spawnAmount < amountToSpawn)
        {
            GameObject cube;
            cube = Instantiate(cubeprefab, transform.position, Quaternion.identity) as GameObject;
            cube.transform.SetParent(transform);
            spawnAmount++;
        }
    }
}

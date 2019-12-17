using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEst : MonoBehaviour
{
	[SerializeField]
	Number number_;

    // Start is called before the first frame update
    void Start()
    {
		//number_.SetNum(0);
	//	number_.SetNum(19);
    }

    // Update is called once per frame
    void Update()
    {
		number_.SetNum(Random.Range(0, 100));
    }
}

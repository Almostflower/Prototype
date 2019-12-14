using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainScript : MonoBehaviour
{
    public Texture texture;

    private float brainmove = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var material = this.GetComponent<Renderer>().material;

        brainmove += 0.01f * Time.deltaTime;
        if (brainmove < 0.02f)
        {
            brainmove += 0.024f * Time.deltaTime;
        }
        if(brainmove >= 0.06f)
        {
            brainmove = 0.0f;
        }

        // Height Map
        material.SetTexture("_ParallaxMap", texture);
        material.SetFloat("_Parallax", brainmove);
    }
}

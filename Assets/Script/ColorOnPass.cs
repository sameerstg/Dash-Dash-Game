using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOnPass : MonoBehaviour
{
    public Color changeColor;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerMovement._instance.transform.position.x >=transform.position.x-2)
        {
            GetComponent<Renderer>().material.color = changeColor;
        }
    }
}

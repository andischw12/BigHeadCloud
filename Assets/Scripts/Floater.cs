using UnityEngine;
using System;
using System.Collections;

public class Floater : MonoBehaviour
{
    float originalY;

    public float floatStrength = 1; // You can change this in the Unity Editor to 
                                    // change the range of y positions that are possible.

    void Start()
    {
        floatStrength = 0.1f;
        this.originalY = this.transform.position.y;
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
        //transform.Rotate(0, 1, 0);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateYAxis : MonoBehaviour
{
    public float transformSpeed = 0.001f;
    public float yPoint = 0.0f;
    public int moves = 0;

    // Update is called once per frame
    void Update()
    {
        if(moves < 40)
        {
            yPoint = transform.position.y + transformSpeed;
            moves++;
        }
        else if(moves < 80)
        {
            yPoint = transform.position.y - transformSpeed;
            moves++;
        }
        else
        {
            moves = 0;
        }
        transform.position = new Vector3(transform.position.x, yPoint, transform.position.z);
    }
}

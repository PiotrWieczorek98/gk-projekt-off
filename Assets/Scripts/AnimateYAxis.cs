using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateYAxis : MonoBehaviour
{
    public float transformSpeed = 0.1f;
    public float yPoint = 2.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, (yPoint + Mathf.PingPong(Time.time * transformSpeed, 0.2f)), transform.position.z);
    }
}

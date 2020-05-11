using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour
{
    private float time=0.25f;
    //current state
    private bool isOpen = false;
    //opening object
    public GameObject obj = null;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = (Animator)obj.GetComponent<Animator>();
        Debug.Log("started");
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E)&&time > 0)
        {
            Debug.Log("E");
            if (!isOpen)
            {
                animator.SetTrigger("OpenDoor");
                isOpen = true;
            }
            else
            {
                animator.SetTrigger("CloseDoor");
                isOpen = false;
            }
            time -= Time.deltaTime;
        }
        if (time < 0)
            time = 1f;
    }

    // Update is called once per frame
    void Update()
    {
    }
}

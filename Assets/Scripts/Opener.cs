using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour
{
    //current state of object
    private bool isOpen = false;
    public Animator animator;
    private bool inTrigger = false;


    // Start is called before the first frame update
    void Start()
    {
        //obj = (GameObject)gameObject.GetComponent<GameObject>;
        animator = (Animator)this.GetComponent<Animator>();
        Debug.Log("started");
        //time = 1f;
    }

    private void OnTriggerStay(Collider other)
    {
        //time += Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.E)&&time > 0)
        //{
        //    Debug.Log("E");
        //    if (!isOpen)
        //    {
        //        animator.SetTrigger("OpenDoor");
        //        isOpen = true;
        //    }
        //    else
        //    {
        //        animator.SetTrigger("CloseDoor");
        //        isOpen = false;
        //    }
        //    time -= Time.deltaTime;
        //}
        //if (time < 0)
        //    time = 1f;
        inTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inTrigger)
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
            //time -= Time.deltaTime;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opener : MonoBehaviour
{
    private bool isOpen;
    private Animator animator;


    void Start()
    {
        animator = (Animator)this.GetComponent<Animator>();
        isOpen = animator.GetBool("state");

    }


    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player") )
        {
            if (!isOpen)
            {
                animator.SetBool("state", true);
                isOpen = true;
            }
            else
            {
                animator.SetBool("state", false);
                isOpen = false;
            }
            //time -= Time.deltaTime;
        }
    }
}

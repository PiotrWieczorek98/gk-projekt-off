using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnClick : MonoBehaviour
{
    private bool isClicked;
    private Animator animator;


    void Start()
    {
        animator = (Animator)this.GetComponent<Animator>();
        isClicked = animator.GetBool("state");

    }


    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player"))
        {
            if (!isClicked)
            {
                animator.SetBool("state", true);
                isClicked = true;
            }
            //time -= Time.deltaTime;
        }
    }
}

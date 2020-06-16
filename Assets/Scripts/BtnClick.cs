using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour
{
    private bool isClicked = false;
    public int iterator = 0;
    public Image endScreen;
    public Button endButton;
    //private Animator animator;


    void Start()
    {
        //animator = (Animator)this.GetComponent<Animator>();
        //isClicked = animator.GetBool("state");
        endScreen.enabled = false;
        endButton.enabled = false;
    }


    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.CompareTag("Player"))
        {
            if (!isClicked)
            {
                //animator.SetBool("state", true);
                isClicked = true;
            }
            //time -= Time.deltaTime;
        }

        if (isClicked)
            iterator++;

        if (iterator == 300)
        {
            endScreen.enabled = true;
            endButton.enabled = true;
        }
            
    }
}

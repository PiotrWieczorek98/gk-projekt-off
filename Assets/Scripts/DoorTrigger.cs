using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    //Door animator
    public Animator animator;

    //Door object
    public Door door;



    // Start is called before the first frame update
    void Start()
    {
        animator = (Animator)door.GetComponent<Animator>();
    }


    private void OnTriggerStay(Collider other)
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E");

            if (!door.isOpened())
            {
                animator.SetTrigger("OpenDoor");
                door.isOpened(true);
            }
            else
            {
                animator.SetTrigger("CloseDoor");
                door.isOpened(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

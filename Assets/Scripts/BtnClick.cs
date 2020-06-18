using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour
{
    private bool isClicked = false;
    public int iterator = 0;
    public Image endScreen;
    private Animator animator;

    public GameManager gameManager;

    void Start()
    {
        isClicked = animator.GetBool("state");
        endScreen.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E) && other.gameObject.tag == "Player")
        {
            if (!isClicked)
            {
                isClicked = true;
            }
        }

        if (isClicked)
            gameManager.CompleteLevel();
    }
}

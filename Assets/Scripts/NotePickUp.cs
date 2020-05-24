using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotePickUp : MonoBehaviour
{
    public Image noteImg;

    // Start is called before the first frame update
    void Start()
    {
        noteImg.enabled = false;    // At the beggining note is not displayed
    }

    public void ShowNoteImg()
    {
        noteImg.enabled = true;
    }
    
    public void HideNoteImg()
    {
        noteImg.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowNoteImg();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideNoteImg();
        }
    }
}

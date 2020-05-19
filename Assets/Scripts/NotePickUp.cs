using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotePickUp : MonoBehaviour
{
    int iterator = 0;
    public Image noteImg;

    public AudioClip interactionSound;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        noteImg.enabled = false;    // At the beggining note is not displayed
    }

    public void ShowNoteImg()
    {
        noteImg.enabled = true;
        GetComponent<AudioSource>().PlayOneShot(interactionSound);
    }
    
    public void HideNoteImg()
    {
        noteImg.enabled = false;
        GetComponent<AudioSource>().PlayOneShot(interactionSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source = other.GetComponent<AudioSource>();
            ShowNoteImg();
        }
    }

    private void Update()
    {
        if(noteImg.enabled)
        {
            iterator++;
            if(iterator > 700)
            {
                HideNoteImg();
                Destroy(this.gameObject);
            }

        }
    }
}

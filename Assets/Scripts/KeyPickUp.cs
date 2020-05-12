using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public AudioClip pickUpSound;
    public FlashScreen flashScreen;

    AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))           
        {
            source = other.GetComponent<AudioSource>();
            flashScreen.flash("gold");

            source.PlayOneShot(pickUpSound);
            Destroy(this.gameObject);
        }
    }
}

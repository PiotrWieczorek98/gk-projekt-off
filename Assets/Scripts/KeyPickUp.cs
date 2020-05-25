using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public AudioClip pickUpSound;
    public FlashScreen flashScreen;
    [Tooltip("Door unlocked by this key")]
    public Transform doorObject;

    AudioSource source;
    void Start()
    {
        foreach (Transform child in doorObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))           
        {
            source = other.GetComponent<AudioSource>();
            flashScreen.flash("gold");

            source.PlayOneShot(pickUpSound);
            foreach (Transform child in doorObject.transform)
            {
                child.gameObject.SetActive(true);
            }

            Destroy(this.gameObject);
        }
    }
}

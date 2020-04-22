using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public AudioClip shotSound;
    AudioSource source;
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.LookAt(player);

        source = GetComponent<AudioSource>();
        source.PlayOneShot(shotSound);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            other.SendMessage("gotHit", damage, SendMessageOptions.DontRequireReceiver);

        Destroyer.Destroy(this.gameObject);

    }
}

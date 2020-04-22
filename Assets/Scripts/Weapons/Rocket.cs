using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [HideInInspector] public float radius;
    [HideInInspector] public float damage;
    [HideInInspector] public LayerMask layerMask;
    [HideInInspector] public GameObject explosion;

    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("Temp").transform;
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), GetComponent<Collider>());
    }
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Collider[] hitColliders = Physics.OverlapSphere(contact.point, radius, layerMask);

        GameObject newExplosion = Instantiate(explosion, contact.point, Quaternion.identity) as GameObject;
        newExplosion.transform.parent = GameObject.FindGameObjectWithTag("Temp").transform;

        foreach(Collider col in hitColliders)
            col.SendMessage("gotHit", damage, SendMessageOptions.DontRequireReceiver);

        Destroy(this.gameObject);

    }
}

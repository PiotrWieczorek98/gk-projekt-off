using UnityEngine;
using UnityEngine.UI;

public class ProjectileWeapon : MonoBehaviour
{
    public GameObject rocket;
    public GameObject explosion;

    public AudioClip shotSound;
    AudioSource source;

    public int ammoAmount;
    public float rocketForce;
    public float explosionRadius;
    public float explosionDamage;

    public LayerMask layerMask;
    public Text ammoCounter;
    Animator animator;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        ammoCounter.text = ammoAmount.ToString();
        if (Input.GetButtonDown("Fire1") && ammoAmount > 0 && animator.GetBool("fire") == false)
        {
            ammoAmount--;

            //Shot animation + dynamic crosshair spread
            animator.SetBool("fire", true);
            DynamicCrosshair.changeSpread("shot");

            //Shot sound
            if (source.isPlaying)
                source.Stop();
            source.PlayOneShot(shotSound);

            //Shot
            GameObject newRocket = Instantiate(rocket, GameObject.Find("Player Spawn Point").transform.position, Quaternion.identity);
            newRocket.GetComponent<Rocket>().damage = explosionDamage;
            newRocket.GetComponent<Rocket>().radius = explosionRadius;
            newRocket.GetComponent<Rocket>().damage = explosionDamage;
            newRocket.GetComponent<Rocket>().explosion = explosion;
            newRocket.GetComponent<Rocket>().layerMask = layerMask;

            Rigidbody rocketRb = newRocket.GetComponent<Rigidbody>();
            rocketRb.AddForce(Camera.main.transform.forward * rocketForce, ForceMode.Impulse);
        }

    }

    void animationEnded(string boolName)
    {
        if (boolName == "fire")
            animator.SetBool("fire", false);
    }
    public void addAmmo(int amount)
    {
        ammoAmount += amount;
    }

}


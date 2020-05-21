using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHP;
    float currentHP;

    EnemyStates es;
    NavMeshAgent nma;
    BoxCollider bc;
    Rigidbody rb;
    GameObject vision;
    Animator anim;
    AudioSource source;
    public AudioClip death;
    bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        es = GetComponent<EnemyStates>();
        nma = GetComponent<NavMeshAgent>();
        bc = transform.Find("Soldier Body").gameObject.GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        vision = transform.Find("Soldier Head").gameObject;
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void gotHit(float damage)
    {
        if(isDead == false)
        {
            currentHP -= damage;
            if (currentHP <= 0)
            {
                isDead = true;
                es.enabled = false;
                nma.enabled = false;

                //anim.Play("Death");
                source.PlayOneShot(death);

                bc.center = new Vector3(0, -0.8f, 0);
                bc.size = new Vector3(1f, 0.5f, 0.1f);
                Physics.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), bc);

                vision.SetActive(false);
            }
        }  
    }
}

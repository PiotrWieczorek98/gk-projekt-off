using UnityEngine;
using UnityEngine.AI;

public class EnemyStates : MonoBehaviour
{
    [HideInInspector] public AlertState alertState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public IEnemyAI currentState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Vector3 targetLastKnownPosition;
    [HideInInspector] public Animator anim;

    public bool meleeOnly = false;
    public float fov;
    public float stayAlertTime;
    public int visionRange;
    public int shootRange;
    public int meleeRange;
    public int shootDamage;
    public int meleeDamage;

    public GameObject bullet;
    public float bulletSpeed;
    public float shotDelay;
    public float meleeDelay;

    public LayerMask raycastMask;
    public Transform chaseTarget;
    public Transform[] waypoints;
    public Transform vision;

    AudioSource source;
    public AudioClip spotted;
    [HideInInspector] public bool isSpotted = false;
    void Awake()
    {
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        currentState = patrolState;
    }

    void Update()
    {
        currentState.UpdateActions();
    }

    void OnTriggerEnter(Collider otherObj)
    {
        currentState.OnTriggerEnter(otherObj);
    }

    void alertByShot(Vector3 shotPosition)
    {
        targetLastKnownPosition = shotPosition;
        currentState = alertState;
    }

    public bool enemySpotted()
    {
        Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        float angle = Vector3.Angle(direction, vision.forward);

        if(angle < fov * 0.5f)
        {
            RaycastHit hit;
            if(Physics.Raycast(vision.transform.position, direction.normalized, out hit, visionRange ,raycastMask))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    chaseTarget = hit.transform;
                    targetLastKnownPosition = hit.transform.position;

                    if(source.isPlaying == false && isSpotted == false)
                    {
                        source.PlayOneShot(spotted);
                        isSpotted = true;
                    }

                    return true;
                }
            }
        }
        return false;
    }
}

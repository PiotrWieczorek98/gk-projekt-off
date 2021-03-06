﻿using System;
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
    
    public Animator anim;
    public float fov;
    public float stayAlertTime;
    public int visionRange;
    public float rotateSpeed;


    [System.Serializable]
    public class AttackSettings
    {
        public bool meleeOnly = false;
        public int shootRange;
        public int meleeRange;
        public int meleeDamage;
        public Transform bulletPrefab;
        public float bulletForce;
        public float bulletDamage = 1;
        public float bulletSpread;
        public float shotDelay;
        public int bulletsInSeries;
        public float seriesDelay;
        public float meleeDelay;
        public Transform bulletSpawnPoint;
    }
    public AttackSettings attackSettings;

    public LayerMask raycastMask;
    public Transform chaseTarget;
    public Transform[] waypoints;
    public Transform vision;

    public AudioSource source;
    public AudioClip spottedSound;
    public AudioClip shotSound;

    [HideInInspector] public bool isSpotted = false;
    void Awake()
    {
        alertState = new AlertState(this);
        attackState = new AttackState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        // Draw a ray
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);

        Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - vision.position;

        float angle = Vector3.Angle(direction, vision.forward);

        if (angle < fov * 0.5f)
        {

            RaycastHit hitinfo;
            if (Physics.Raycast(vision.position, direction.normalized, out hitinfo, visionRange, raycastMask))
            {
                if (hitinfo.collider.CompareTag("Player"))
                {

                    chaseTarget = hitinfo.transform;
                    targetLastKnownPosition = hitinfo.transform.position;

                    if(source.isPlaying == false && isSpotted == false)
                    {
                        source.PlayOneShot(spottedSound);
                        isSpotted = true;

                    }

                    return true;
                }
            }
        }
        return false;
    }
}

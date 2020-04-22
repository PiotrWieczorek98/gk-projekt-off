using System;
using UnityEngine;

public class PatrolState : IEnemyAI
{
    EnemyStates enemy;
    int nextWaypoint = 0;
    public PatrolState(EnemyStates _enemy)
    {
        enemy = _enemy;
    }

    public void UpdateActions()
    {
        watch();
        patrol();
        enemy.isSpotted = false;
    }

    private void patrol()
    {
        enemy.navMeshAgent.destination = enemy.waypoints[nextWaypoint].position;
        enemy.navMeshAgent.isStopped = false;

        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % enemy.waypoints.Length;
        }
    }

    void watch()
    {
        if (enemy.enemySpotted())
            ToChaseState();
    }

    public void OnTriggerEnter(Collider enemy)
    {
        if(enemy.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }




    public void ToPatrolState()
    {
        //Current State
    }
    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }
    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }
    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
}

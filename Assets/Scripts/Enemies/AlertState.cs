using System;
using UnityEngine;

public class AlertState : IEnemyAI
{
    EnemyStates enemy;
    float timer = 0;
    public AlertState(EnemyStates _enemy)
    {
        enemy = _enemy;
    }
    public void UpdateActions()
    {
        search();
        watch();
        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
            lookAround();
    }

    void lookAround()
    {
        timer += Time.deltaTime;
        if(timer >= enemy.stayAlertTime)
        {
            timer = 0;
            ToPatrolState();
        }
    }

    void watch()
    {
        if (enemy.enemySpotted())
        {
            enemy.navMeshAgent.destination = enemy.targetLastKnownPosition;
            ToChaseState();
        }

    }

    void search()
    {
        enemy.navMeshAgent.destination = enemy.targetLastKnownPosition;
        enemy.navMeshAgent.isStopped = false;
    }

    public void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.CompareTag("Player"))
        {
            ToAttackState();
        }
    }




    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }
    public void ToAttackState()
    {
        //Not possible
    }
    public void ToAlertState()
    {
        //Current state
    }
    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }
}

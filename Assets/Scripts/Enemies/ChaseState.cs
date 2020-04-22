using System;
using UnityEngine;

public class ChaseState : IEnemyAI
{
    EnemyStates enemy;
    public ChaseState(EnemyStates _enemy)
    {
        enemy = _enemy;
    }
    public void UpdateActions()
    {
        watch();
        chase();
    }

    void chase()
    {
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false;

        if (enemy.navMeshAgent.remainingDistance <= enemy.meleeRange && enemy.meleeOnly == true)
        {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        }
        else if (enemy.navMeshAgent.remainingDistance <= enemy.shootRange && enemy.meleeOnly == false)
        {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        }

    }

    void watch()
    {
        if (enemy.enemySpotted() == false)
            ToAlertState();
    }

    public void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }




    public void ToPatrolState()
    {
        //Not possible
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
        //Current state
    }
}

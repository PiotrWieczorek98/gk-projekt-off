using UnityEngine;

public class AttackState : IEnemyAI
{
    EnemyStates enemy;
    float attackTimer;
    float distance;


    public AttackState(EnemyStates _enemy)
    {
        enemy = _enemy;
    }
    public void UpdateActions()
    {
        attackTimer += Time.deltaTime;
        distance = Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position);
        if(distance > enemy.meleeRange && enemy.meleeOnly == true)
        {
            ToChaseState();
        }
        else if(distance > enemy.shootRange && enemy.meleeOnly == false)
        {
            ToChaseState();
        }
        watch();

        if(distance<=enemy.shootRange && distance > enemy.meleeRange && enemy.meleeOnly == false && attackTimer >= enemy.shotDelay)
        {
            attack(true);
            attackTimer = 0;
        }
        else if(distance <=enemy.meleeRange && attackTimer >= enemy.meleeDelay)
        {
            attack(false);
            attackTimer = 0;
        }
    }

    void attack(bool shoot)
    {
        //Attack sprite
        enemy.anim.Play("Attack");

        if (shoot == false)
        {
            enemy.chaseTarget.SendMessage("gotHit", enemy.meleeDamage, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
           GameObject bullet = GameObject.Instantiate(enemy.bullet, enemy.transform.Find("Spawn Point").position, Quaternion.identity);
            bullet.GetComponent<Bullet>().speed = enemy.bulletSpeed;
            bullet.GetComponent<Bullet>().damage = enemy.shootDamage;
        }
    }

    void watch()
    {
        if (enemy.enemySpotted() == false)
            ToAlertState();
    }

    public void OnTriggerEnter(Collider enemy)
    {

    }




    public void ToPatrolState()
    {
        //Not possible
    }
    public void ToAttackState()
    {
        //CurrentState
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

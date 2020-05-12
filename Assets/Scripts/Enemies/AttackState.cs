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



        if ((distance > enemy.shootRange && enemy.meleeOnly == false) || 
            (distance > enemy.meleeRange && enemy.meleeOnly == true))
            ToChaseState();
        
        watch();

        if(distance <= enemy.shootRange && distance > enemy.meleeRange && enemy.meleeOnly == false && attackTimer >= enemy.shotDelay)
        {
            attack(true);
            attackTimer = 0;
        }
        else if(distance <= enemy.meleeRange && attackTimer >= enemy.meleeDelay)
        {
            attack(false);
            attackTimer = 0;
        }

        // Rotate to player
        float singleStep = enemy.rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 targetDirection = enemy.chaseTarget.position - enemy.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(enemy.transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(enemy.transform.position, newDirection * 10, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        enemy.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void attack(bool shoot)
    {
        enemy.anim.SetBool("attack", true);

        if (shoot == false)
        {
            enemy.chaseTarget.SendMessage("gotHit", enemy.meleeDamage, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            enemy.source.PlayOneShot(enemy.shotSound);

            var bullet = Transform.Instantiate(
                enemy.bulletPrefab,
                enemy.bulletSpawnPoint.transform.position,
                enemy.bulletSpawnPoint.transform.rotation);

            //Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity =
            bullet.transform.forward * enemy.bulletForce;

            // Add values
            bullet.tag = "Bullet";
			bullet.gameObject.layer = LayerMask.NameToLayer("Enemy");
            bullet.GetComponent<BulletScript>().damage = enemy.bulletDamage;
        }
    }

    void watch()
    {
        if (enemy.enemySpotted() == false)
            ToAlertState();
        enemy.anim.SetBool("attack", false);

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
        enemy.anim.SetBool("attack", false);

    }
    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
        enemy.anim.SetBool("attack", false);

    }
}

using UnityEngine;

public class AttackState : IEnemyAI
{
    EnemyStates enemy;
    float attackTimer;
    float seriesTimer;
    int bulletsCount;
    float distance;


    public AttackState(EnemyStates _enemy)
    {
        enemy = _enemy;
    }
    public void UpdateActions()
    {
        attackTimer += Time.deltaTime;
        distance = Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position);

        if ((distance > enemy.attackSettings.shootRange && enemy.attackSettings.meleeOnly == false) || 
            (distance > enemy.attackSettings.meleeRange && enemy.attackSettings.meleeOnly == true))
            ToChaseState();
        
        watch();

        if(distance <= enemy.attackSettings.shootRange && distance > enemy.attackSettings.meleeRange && 
            enemy.attackSettings.meleeOnly == false && 
            attackTimer >= enemy.attackSettings.shotDelay && 
            bulletsCount < enemy.attackSettings.bulletsInSeries)
        {
            attack(true);
            attackTimer = 0;
            bulletsCount++;
        }
        else if(distance <= enemy.attackSettings.meleeRange && 
            attackTimer >= enemy.attackSettings.meleeDelay)
        {
            attack(false);
            attackTimer = 0;
        }
        else if(bulletsCount >= enemy.attackSettings.bulletsInSeries)
        {
            seriesTimer += Time.deltaTime;
            if(seriesTimer >= enemy.attackSettings.seriesDelay)
            {
                seriesTimer = 0;
                bulletsCount = 0;
            }
        }

        // Rotate to player
        float singleStep = enemy.rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 targetDirection = enemy.chaseTarget.position - enemy.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(enemy.transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        enemy.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void attack(bool shoot)
    {
        enemy.anim.SetBool("attack", true);

        if (shoot == false)
        {
            enemy.chaseTarget.SendMessage("gotHit", enemy.attackSettings.meleeDamage, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            enemy.source.PlayOneShot(enemy.shotSound);

            var bullet = Transform.Instantiate(
                enemy.attackSettings.bulletPrefab,
                enemy.attackSettings.bulletSpawnPoint.transform.position,
                enemy.attackSettings.bulletSpawnPoint.transform.rotation);

            //Bullet spread
            float strayFactor = enemy.attackSettings.bulletSpread;
            var randomNumberX = Random.Range(-strayFactor, strayFactor);
            var randomNumberY = Random.Range(-strayFactor, strayFactor);
            var randomNumberZ = Random.Range(-strayFactor, strayFactor);
            bullet.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);

            //Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity =
            bullet.transform.forward * enemy.attackSettings.bulletForce;


            // Add attributes
            bullet.tag = "Bullet";
			bullet.gameObject.layer = LayerMask.NameToLayer("Enemy");
            bullet.GetComponent<BulletScript>().damage = enemy.attackSettings.bulletDamage;
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

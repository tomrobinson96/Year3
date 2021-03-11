using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    
    public float timeBetweenAttacks = 0.5f;     // Time (in seconds) between each attack.
    public int attackDamage = 10;               // The amount of health taken away per attack.
    public BoxCollider attackRange;             // range of player attck
    
    GameObject enemy;                          // Reference to the enemy GameObject.
    EnemyControl enemyHealth;                  // Reference to the enemy's health.
    bool enemyInRange;                         // Whether enemy is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
    Animation anim;
    public Collider closestEnemy;


    void Awake()
    {
        // Setting up references.
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        anim = GetComponent<Animation>();
        enemyHealth = enemy.GetComponent<EnemyControl>();
    }


    void OnTriggerExit(Collider other)


    {
        // If the exiting collider is the enemy...
        if (other.gameObject.tag == "Enemy")
        {
            // ... the enemy is no longer in range.
            enemyInRange = false;
        }
    }



    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks and the enemy is in range and mouse is pressed...
        if (timer >= timeBetweenAttacks && enemyInRange && Input.GetButtonDown("fire1"))
        {
            // ... attack.
            Debug.Log("attack");
            Attack(closestEnemy);
        }
        
    }

    void Attack(Collider closestEnemy1)
    {
        //closestEnemy1 = closestEnemy;
        // Reset the timer.
        timer = 0f;       

        // If the enemy has health to lose...
        if (enemyHealth.currentHealth > 0)
        {
            // ... damage the enemy.
            enemyHealth.Damage(attackDamage);
        }
    }
}

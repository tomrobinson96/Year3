using UnityEngine;
using System.Collections;


public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
    public int attackDamage = 10;               // The amount of health taken away per attack.
    public BoxCollider attackRange;             // Range the enemy can attack in
    //Reference to question canvas'
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;

    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
    float timer;                                // Timer for counting up to the next attack.
    Animation anim;


    void Awake()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animation>();
        playerHealth = player.GetComponent<PlayerHealth>();
        playerInRange = false;
    }


    void OnTriggerEnter(Collider other)
    {

        // If the entering collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)


    {
        // If the exiting collider is the player...
        if (other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }



    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the timer exceeds the time between attacks and the player is in range ...
        if (timer >= timeBetweenAttacks && playerInRange)
        {
            // ... attack.
            Attack();
        }

        // If the player has zero or less health...
        if (playerHealth.currentHealth <= 0)
        {
            // ... tell the animator the player is dead.
            //anim.SetTrigger("PlayerDead");
        }
        if (canvas1.activeInHierarchy || canvas2.activeInHierarchy || canvas3.activeInHierarchy)
        {
            playerInRange = false;
        }
    }


    void Attack()
    {
        // Reset the timer.
        timer = 0f;

        anim.Play("strike");

        // If the player has health to lose...
        if (playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage(attackDamage);
        }
    }
}

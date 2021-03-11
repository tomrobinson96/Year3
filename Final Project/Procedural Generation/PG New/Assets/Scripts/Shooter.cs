using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{

    //Stats for the bullet
    public Rigidbody projectile;
    public float speed = 20;
    public float timeBetweenShots;
    bool inRange;
    float timer;

    //Reference to Weapons
    public GameObject gun, sword, longsword;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        /* If swords are active in the game, mouse is clicked, is in range
          and time is more than the time between attacks variable then attack */
        if(sword.activeInHierarchy || longsword.activeInHierarchy)
        {
            if (Input.GetButtonDown("fire1") && inRange && timer < timeBetweenShots)
            {
                Debug.Log("shoot");
                Shoot();
            }
        }
        // If gun is active in game and mouse is down then shoot projectile
        if (gun.activeInHierarchy)
        {
            if (Input.GetButtonDown("fire1"))
            {
                Debug.Log("shoot");
                Shoot();
            }
        }
    }

    // Setting 'in range' true or false
    void OnTriggerEnter(Collider other)
    {

        // If the entering collider is an enemy...
        if (other.gameObject.tag == "Enemy")
        {
            // ... an enemy is in range.
            inRange = true;            
            Debug.Log("inRange");
        }
    }

    //Shooting method
    void Shoot()
    {
        {
            Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
            instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, speed, 0));
            timer = 0;
        }

    }

}

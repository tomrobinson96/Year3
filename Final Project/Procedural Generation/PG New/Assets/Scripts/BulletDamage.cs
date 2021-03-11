using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

    /*This script is designed to hold the value damage that a bullet will 
        inflict on a enemy, it will also bestroy any bullets that hit floors,
        walls or enemies*/

    public int damagePerHit = 50;
    public int bulletDamage = 80;

    void Update()
    {
        Destroy(gameObject, 1);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyControl enemy = collision.gameObject.GetComponent<EnemyControl>();
        if (enemy != null)
        {

            // Kill the enemy
            EnemyControl enemyHealth = enemy.GetComponent<EnemyControl>();
            if (enemyHealth != null)
                enemyHealth.Damage(bulletDamage);
        }
        Destroy(gameObject);
    }
}

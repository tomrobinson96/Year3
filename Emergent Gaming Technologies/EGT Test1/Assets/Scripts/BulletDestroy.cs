using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {


    /*This scropt is designed to hold the value damage that a bullet will 
    inflict on a zombie, it will also bestroy any bullts that hit floors,
    walls or enemies*/

    public int damagePerHit = 50;    

    void Update () {
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
                enemyHealth.Damage(enemyHealth.currentHealth);
            
        }
        Destroy(gameObject);
    }
}

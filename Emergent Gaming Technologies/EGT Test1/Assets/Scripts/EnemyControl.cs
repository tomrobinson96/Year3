using UnityEngine;
using System.Collections;
using System;
/* This script is used to control the enemies movement, which will
   be activated when the player enters a box collider, it also 
   contains the function to take health away from the enemies*/

public class EnemyControl : MonoBehaviour {


    public BoxCollider territory;
    GameObject player;
    private Transform myTransform;
    bool playerInTerritory;
    public Transform Player;
    Animation anim;

    public int startingHealth = 100;
    public int currentHealth;
    public float moveSpeed = 0.5f;    
     

    void Awake()
    {
        myTransform = transform;
        currentHealth = startingHealth;
        anim = GetComponent<Animation>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Use this for initialization
    void Start()
    {
        playerInTerritory = false;        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerInTerritory == true)
        {
            Vector3 lookAt = Player.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            anim.Play("walk");
        }

        if (playerInTerritory == false)
        {
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTerritory = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInTerritory = false;
        }
    }
    public void Damage(int damageCount)
    {
        currentHealth -= damageCount;

        if (currentHealth <= 0)
        {
            // Dead!
            Destroy(gameObject);
            return;
        }
    }

}
    


   




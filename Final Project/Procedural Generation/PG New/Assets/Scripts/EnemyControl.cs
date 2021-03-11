using UnityEngine;
using System.Collections;
using System;

public class EnemyControl : MonoBehaviour {

    public Collider territory;      //Space for enemy when movement is active
    GameObject player;              //Gameobject for player
    private Transform myTransform;  //Access transform for movement
    bool playerInTerritory;         //Player in territory?
    public Transform Player;        //Access player transform to check if in territory
    Animation anim;
    bool endGame;

    // Reference to question canvas'
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;

    //Enemy stats
    public int startingHealth = 200;
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
        //if player in territory make enemy face player and move towards it
        if (playerInTerritory == true)
        {
            Vector3 lookAt = Player.position;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            anim.Play("move");
        }
        
        //if not in territory just look at player 
        if (playerInTerritory == false)
        {
            Vector3 lookAt = Player.position;
        }

        //if questions are active then stop movement
        if(canvas1.activeInHierarchy || canvas2.activeInHierarchy || canvas3.activeInHierarchy)
        {
            playerInTerritory = false;
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

    // ********* Method taking damage from attack amount away from current health ******** \\
    public void Damage(int damageCount)
    {
        
            currentHealth -= damageCount;
            Debug.Log(currentHealth);
        

        if (currentHealth <= 0)
        {
            // Dead!
            gameObject.SetActive(false);
            Debug.Log("DEad");
            Debug.Log(currentHealth);
            return;
        }
    }
  }

using UnityEngine;
using System.Collections;

public class MissionDrop : MonoBehaviour {

    /* This script controls how many boxes will be swpaned and 
       what happens after the player has answered the questions correctly */

    // References for spawning questions
    public Transform missionDrop;
    public int numToSpawn;
    public Transform[] spawnPoints; 
       
    // Bools that track which answers have been answered correctly
    bool answered1 = false;
    bool answered2 = false;
    bool answered3 = false;

    //Reference to weapons and questions
    public GameObject sword;
    public GameObject empty;
    public GameObject longSword;
    public GameObject gun;
    public GameObject helloWorld;
    public GameObject welcomeMessage;
    public GameObject questionnaire;
    public GameObject trolls;
    public GameObject bossTroll;
    public GameObject winnerScreen;

    

    void Start()
    {
        int spawned = 0;
        while (spawned < numToSpawn)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            Instantiate(missionDrop, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
            spawned++;
        }
        
        
    }
    public void Answered()
    {
        answered1 = true;
        StartCoroutine(ExecuteAfterTime(2));
        empty.SetActive(false);
        helloWorld.SetActive(false);
    }

    public void Answered2()
    {
        answered2 = true;
        sword.SetActive(false);
        StartCoroutine(ExecuteAfterTime2(2));
        welcomeMessage.SetActive(false);
    }

    public void Answered3()
    {
        answered3 = true;
        longSword.SetActive(false);
        gun.SetActive(true);
        questionnaire.SetActive(false);
    }
    void SpawnBoss()
    {
        if(answered1 == true && answered2 == true && answered3 == true )
        {
            bossTroll.SetActive(true);
        }
    }

    // Setting the next weapon active after a certain time to stop the next question spawning straight away
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        sword.SetActive(true);
    }
    // Setting the next weapon active after a certain time to stop the next question spawning straight away
    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);
        longSword.SetActive(true);
    }

    void Update()
    {
        // If the boss troll has been destoryed the player has won
        if(bossTroll == null)
        {
            winnerScreen.SetActive(true);            
        }
        
    }
}

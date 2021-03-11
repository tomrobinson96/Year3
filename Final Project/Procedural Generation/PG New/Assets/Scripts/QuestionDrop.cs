using UnityEngine;
using System.Collections;

public class QuestionDrop : MonoBehaviour {

    /* This script is designed to decide what question
       will be displayed when the player approaches a box 
       within the game */

    //Reference to weapons and questions
    public GameObject empty;
    public GameObject sword;
    public GameObject longSword;
    public GameObject gun;
    public GameObject helloWorld;
    public GameObject welcomeMessage;
    public GameObject multiChoice;
    public GameObject winScreen;
    public GameObject pauseMenu;

    void Update()
    {
        // If question is active then disable player movement, enemy attacking and pause the environment as it is
        if (helloWorld.activeInHierarchy == true || welcomeMessage.activeInHierarchy == true || multiChoice.activeInHierarchy == true || winScreen.activeInHierarchy == true || pauseMenu.activeInHierarchy == true )
        {
            GetComponent<BasicPlayer>().enabled = false;
            GetComponent<EnemyControl>().enabled = false;
            //GetComponent<Shooter>().enabled = false;
            Time.timeScale = 0f;
        }
        
        //If returning to the game resume player movement and unpause the environment
        if (helloWorld.activeInHierarchy != true || welcomeMessage.activeInHierarchy != true || multiChoice.activeInHierarchy != true || winScreen.activeInHierarchy != true || pauseMenu.activeInHierarchy != true)
        {
            GetComponent<BasicPlayer>().enabled = true;
           // GetComponent<Shooter>().enabled = true;
            Time.timeScale = 1f;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Mission")
        {
            if (empty.activeInHierarchy == true)
            {
                helloWorld.SetActive(true);
            }
            Destroy(other);
        }
        if (other.gameObject.tag == "Mission")
        {
            if (sword.activeInHierarchy == true)
            {
               // helloWorld.SetActive(false);
                welcomeMessage.SetActive(true);
            }
            Destroy(other);
        }
        if (other.gameObject.tag == "Mission")
        {
            if (longSword.activeInHierarchy == true)
            {
                multiChoice.SetActive(true);
            }
            Destroy(other);
        }
    }
}

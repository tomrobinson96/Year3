using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    /* ************* This script handles the movement of levels and
    the movement between certain canvas' ********************** */

    //Gameobjects
    public GameObject info1;
    public GameObject info2;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;
    public GameObject page5;
    public GameObject page6;
    public GameObject pauseMenu;
    public GameObject instructionMenu;

    //Loads Instruction Scene
    public void LoadInstructions(string level)
    {
        SceneManager.LoadScene(1);
    }

    //Loads game
    public void LoadGame(string level)
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1f;
    }

    //Loads Home
    public void LoadHome(string level)
    {
        SceneManager.LoadScene(0);
    }

    public void InfoLoad()
    {
        info1.SetActive(true);
    }
    public void InfoRemove()
    {
        info1.SetActive(false);
    }
    public void InfoLoad2()
    {
        info2.SetActive(true);
    }
    public void InfoRemove2()
    {
        info2.SetActive(false);
    }
    public void Page2Load()
    {
        page2.SetActive(true);
    }
    public void Page3Load()
    {
        page3.SetActive(true);
    }
    public void Page4Load()
    {
        page4.SetActive(true);
    }
    public void Page5Load()
    {
        page5.SetActive(true);
    }
    public void Page6Load()
    {
        page6.SetActive(true);
    }
    public void PauseMenuLoad()
    {
        pauseMenu.SetActive(true);
        instructionMenu.SetActive(false);
    }
    public void PauseMenuRemove()
    {
        pauseMenu.SetActive(false);
        GetComponent<EnemyAttack>().enabled = true;
        GetComponent<EnemyControl>().enabled = true;
        Time.timeScale = 1f;

    }
    public void InstructionMenuLoad()
    {
        instructionMenu.SetActive(true);
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }
}

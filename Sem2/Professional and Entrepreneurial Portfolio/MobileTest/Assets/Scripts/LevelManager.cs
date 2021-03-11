using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject pauseCanvas;

    public void LoadInstructions(string level)
    {
        SceneManager.LoadScene(1);
    }
    public void LoadObjectives(string level)
    {
        SceneManager.LoadScene(5);
        Time.timeScale = 1.0f;
    }
    public void LoadChampionship(string level)
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1.0f;
    }

    public void LoadEndless(string level)
    {        
        SceneManager.LoadScene(3);
        Time.timeScale = 1.0f;
    }

    public void LoadHome(string level)
    {
        SceneManager.LoadScene(0);
    }
    public void LoadChampionship2(string level)
    {        
        SceneManager.LoadScene(4);
        Time.timeScale = 1.0f;
    }
    public void LoadChampionship3(string level)
    {
        SceneManager.LoadScene(6);
        Time.timeScale = 1.0f;
    }
    public void UnPause(string level)
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Quit(string level)
    {
        Application.Quit();
    }


}

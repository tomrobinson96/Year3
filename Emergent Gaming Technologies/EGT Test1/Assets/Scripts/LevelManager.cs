using UnityEngine;
using UnityEngine.SceneManagement;
/* Old level manager*/ 

public class LevelManager : MonoBehaviour {

    public void LoadInstructions(string level)
    {
        SceneManager.LoadScene(1);
    }
    public void LoadInstructions1(string level)
    {
        SceneManager.LoadScene(2);
    }
    public void LoadGame(string level)
    {
        SceneManager.LoadScene(3);
    }

    public void LoadHome(string level)
    {
        SceneManager.LoadScene(0);
    }


    GameObject key;

    public Collider other;

    void Awake()
    {
        key = GameObject.Find("ActualSize");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == key)
        {
            SceneManager.LoadScene(4);
        }
    }
}

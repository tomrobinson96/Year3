using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour {

    
    public GameObject deathCanvas;
    public GameObject deathEndCanvas;
    public GameObject winCanvas;
    public GameObject secondCanvas;
    public GameObject thirdCanvas;
    public GameObject fourthCanvas;
    public GameObject endless;
    public GameObject Championship;

    public int firstPlace;
    public float secondPlace;
    public float thirdPlace;
    public float fourthPlace;

    int overallScore;


    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {

                deathCanvas.SetActive(true);
                StartCoroutine(ExecuteAfterTime2(3));

            
        }
        if(other.gameObject.tag == "Finish1" && timer <= firstPlace)
        {
            winCanvas.SetActive(true);
            Time.timeScale = 0f;
            overallScore =+ 1;
            
        }
        if (other.gameObject.tag == "Finish1" && timer <= secondPlace && timer > firstPlace)
        {
            secondCanvas.SetActive(true);
            Time.timeScale = 0f;
            overallScore =+ 2;
            
        }
        if (other.gameObject.tag == "Finish1" && timer <= thirdPlace && timer > secondPlace)
        {
            thirdCanvas.SetActive(true);
            Time.timeScale = 0f;
            overallScore = +3;
            
        }
        if (other.gameObject.tag == "Finish1" && timer <= fourthPlace && timer > thirdPlace)
        {
            Debug.Log("fourth");
            fourthCanvas.SetActive(true);
            Time.timeScale = 0f;
            overallScore = +4;
            
        }
        if (other.gameObject.tag == "Finish1" && Championship.activeInHierarchy && overallScore ==3 || overallScore == 4)
        {
            SceneManager.LoadScene(7);
        }
        if (other.gameObject.tag == "Finish1" && Championship.activeInHierarchy && overallScore>4 && overallScore<=6)
        {
            SceneManager.LoadScene(8);
        }
        if (other.gameObject.tag == "Finish1" && Championship.activeInHierarchy && overallScore > 6)
        {
            SceneManager.LoadScene(9);
        }
       
    }

    


    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);
        deathEndCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
}

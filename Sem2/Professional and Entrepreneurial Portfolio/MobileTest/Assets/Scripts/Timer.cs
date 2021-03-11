using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    float timeLeft = 30f;    
    int scoreTime;
    int timeScore;
    float score;
    float time;

    //Game Objects
    public GameObject pointsUp;
    public GameObject timeUp;
    public GameObject deathCanvas;
    public GameObject endCanvas;

    //Timer Objects
    public GameObject timer1;
    public GameObject timer2;
    public GameObject timer3;
    public GameObject timer4;
    public GameObject timer5;
    public GameObject timer6;
    public GameObject timer7;
    public GameObject timer8;
    public GameObject timer9;
    public GameObject timer10;
    public GameObject timer11;
    public GameObject timer12;

    //Texts
    public Text timeText;
    public Text scoreText;
    public Text timeLastedText;
    public Text scoreTextDeath;
    public Text timeLastedTextDeath;

    void Update()
    {
        StartCoroutine(CountUpScore(1));
        StartCoroutine(CountUpTime(1));
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Debug.Log("time up");
            StartCoroutine(ExecuteAfterTime(3));            
            StartCoroutine(ExecuteAfterTime2(1));
            

        }
        setCountText();
        setScoreTextDeath();
        setTimeTextDeath();
        setScoreText();
        setTimeText();         
        
        
    }

    void setCountText()
    {
        timeText.text = "Time Left: " + timeLeft.ToString();
    }

    void setScoreText()
    {
        //scoreText.text = "Score Achieved: " + scoreTime.ToString();
    }

    void setTimeText()
    {
//        timeLastedText.text = "Time Lasted: " + timeScore.ToString();
    }
    void setScoreTextDeath()
    {
        score = (scoreTime - timeScore)*10;
       // scoreTextDeath.text = "Score Achieved: " + score.ToString();
    }

    void setTimeTextDeath()
    {
        time = timeScore / 100f;
        //timeLastedTextDeath.text = "Time Lasted: " + time.ToString();
    }

    void OnCollisionEnter2D(Collision2D other)
    {        
        if (other.gameObject.tag == "Time-Up1")
        {
            Debug.Log("addTime");
            Destroy(timer1);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up2")
        {
            Debug.Log("addTime");
            Destroy(timer2);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up3")
        {
            Debug.Log("addTime");
            Destroy(timer3);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up4")
        {
            Debug.Log("addTime");
            Destroy(timer4);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up5")
        {
            Debug.Log("addTime");
            Destroy(timer5);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up6")
        {
            Debug.Log("addTime");
            Destroy(timer6);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up7")
        {
            Debug.Log("addTime");
            Destroy(timer7);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up8")
        {
            Debug.Log("addTime");
            Destroy(timer8);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up9")
        {
            Debug.Log("addTime");
            Destroy(timer9);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up10")
        {
            Debug.Log("addTime");
            Destroy(timer10);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up11")
        {
            Debug.Log("addTime");
            Destroy(timer11);
            timeLeft += 10;
        }
        if (other.gameObject.tag == "Time-Up12")
        {
            Debug.Log("addTime");
            Destroy(timer12);
            timeLeft += 10;
        }
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        endCanvas.SetActive(true);        
        Time.timeScale = 0f;
        

    }
    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);
        deathCanvas.SetActive(true);

    }
    IEnumerator CountUpScore(float time)
    {
        yield return new WaitForSeconds(time);
        scoreTime += 1;
    }
    IEnumerator CountUpTime(float time)
    {
        yield return new WaitForSeconds(time);
        timeScore += 1;
    }

}


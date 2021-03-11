using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class AccelScript : MonoBehaviour {


    public float jumpForce = 1f;
    public float maxSpeed = 10f;   
    bool grounded;
    public int score;
    public Text countText;
    public Text countTextFinal;
    public Text countTextFinal2;
    public Text countTextFinal3;
    public Text countTextFinal1;
    bool beenUpsideDown;
    bool canScore = true;
    public float spinSpeed = 1f;    

    //GameObjects
    public GameObject deathCanvas;
    public GameObject shuvText;
    public GameObject flipText;
    public GameObject pointsUp;
    public GameObject endless;
    public GameObject endCanvas;
    public GameObject pauseCanvas;

    Animator anim;
    Animation clips;    

    void Awake()
    {
        anim = GetComponent<Animator>();
        clips = GetComponent<Animation>();
    }

        void Start()
    {        
        score = 0;
        setCountText();
               
    }    

    void Update()
    {
        //transform.Translate(Input.acceleration.x * (Time.deltaTime * 2), 0, 0 /*-Input.acceleration.z * Time.deltaTime*/);
        transform.Rotate(0, 0, -(Input.acceleration.x * (Time.deltaTime * 400)));
        setCountTextFinal();
        setCountTextFinal1();
        setCountTextFinal2();
        setCountTextFinal3();

        if (Vector2.Dot(transform.up, Vector2.down) > 0)
        {

            beenUpsideDown = true;
        }


        if (beenUpsideDown == true)
        {
            StartCoroutine(ExecuteAfterTime(1f));
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            Jump();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
        }

        if (beenUpsideDown == true && grounded == true)
        {
            Debug.Log("+20");
            score += 20;
            setCountText();
            StartCoroutine(CanScore(2));
            flipText.SetActive(true);
            StartCoroutine(FLipText(2));

        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Shuv();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, spinSpeed * Time.deltaTime * 2);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, -spinSpeed * Time.deltaTime);
        }
        if (canScore == false && grounded == true)
        {
            StartCoroutine(ExecuteAfterTime1(2));
            StartCoroutine(ExecuteAfterTime2(0));
        }       

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Power-Up")
        {
            Destroy(pointsUp);
            score += 100;
            setCountText();

        }
    }
    

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ground")
        {

            grounded = true;
            clips.Play("Rolling");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
        }
     }

    void Jump()
    {
        if (grounded == true)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }
    }
    void Shuv()
    {
        if (grounded == false && canScore == true)
        {
            clips.Play("shuv");
            score += 10;
            setCountText();            
            canScore = false;            
            StartCoroutine(CanScore(1));
            shuvText.SetActive(true);
            StartCoroutine(ShuvText(1));

        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(900, 650, 100, 100), "Ollie"))
        {
            Jump();
        }

        if (GUI.Button(new Rect(900, 540, 100, 100), "Shuv"))
        {
            Shuv();
        }

    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        beenUpsideDown = false;
    }

    IEnumerator ExecuteAfterTime1(float time)
    {
        yield return new WaitForSeconds(time);
        if (endless.activeInHierarchy == true)
        {
            endCanvas.SetActive(true);
        }
        else {
            SceneManager.LoadScene(0);
        }

    }
    IEnumerator ExecuteAfterTime2(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f;
        deathCanvas.SetActive(true);
    }

        IEnumerator CanScore(float time)
    {
        yield return new WaitForSeconds(time);
        canScore = true;
    }

    IEnumerator ShuvText(float time)
    {
        yield return new WaitForSeconds(time);
        shuvText.SetActive(false);
    }

    IEnumerator FLipText(float time)
    {
        yield return new WaitForSeconds(time);
        flipText.SetActive(false);
    }

    void setCountText()
    {
        countText.text = "Score: " + score.ToString();        
    }
    void setCountTextFinal()
    {
        countTextFinal.text = "Score: " + score.ToString();
    }
    void setCountTextFinal1()
    {
        countTextFinal1.text = "Score: " + score.ToString();
    }
    void setCountTextFinal2()
    {
        countTextFinal2.text = "Score: " + score.ToString();
    }
    void setCountTextFinal3()
    {
        countTextFinal3.text = "Score: " + score.ToString();
    }

}

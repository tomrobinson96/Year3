using UnityEngine;
using Leap;
using UnityEngine.SceneManagement;

/* This script is used to control the mouse in the menus
   and to change the scene when the correct objects are hit*/

public class FingerRender : MonoBehaviour
{


    public GameObject start;
    public GameObject instructions;
    public GameObject next;
    public GameObject exit;
    public GameObject home;
    public Rigidbody rb;
    Controller leapController;
    //Assume a reference to the scene HandController object
    public HandController handCtrl;

    void Start()
    {
        leapController = new Controller();
    }

    void Update()
    {
        Frame frame = leapController.Frame();
        Hand hand = frame.Hands.Frontmost;

        if (hand.IsValid)
        {
            rb.MovePosition(
             transform.TransformPoint(hand.PalmPosition.ToUnityScaled()));
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == start)
        { 
            SceneManager.LoadScene(3);
        }
        if (other.gameObject == exit)
        {
            Application.Quit();
        }
        if (other.gameObject == instructions)
        {
            SceneManager.LoadScene(1);
        }
        if (other.gameObject == next)
        {
            SceneManager.LoadScene(2);
        }
        if (other.gameObject == home)
        {
            SceneManager.LoadScene(0);
        }
    }
}


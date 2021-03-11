using UnityEngine;
using Leap;
using System.Collections;

/* This script is used to control the picking up of the 'Grabable'
   objects in the scene when within 'reaching' distance            */

public class KeyPickUp : MonoBehaviour {

    public BoxCollider territory;
    GameObject player;
    private Transform myTransform;
    bool playerInTerritory;
    public Transform Player;
    Controller controller;
    bool pickUp;
    public float moveSpeed = 0.5f;

    void Awake()
    {
        myTransform = transform;      
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Use this for initialization
    void Start()
    {
        playerInTerritory = false;
        controller = new Controller();
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;

        foreach (Hand hand in hands) {

            int extendedFingers = 0;
            for (int f = 0; f < hand.Fingers.Count; f++)
            {
                Finger digit = hand.Fingers[f];
                if (digit.IsExtended)
                    extendedFingers++;
            }


            if (playerInTerritory == true && hands.Count >= 1 && hand.IsLeft && extendedFingers == 0)          
                {
                    pickUp = true;
                    Debug.Log("MoveClose");
                    Vector3 lookAt = Player.position;
                    lookAt.y = transform.position.y;
                    transform.LookAt(lookAt);
                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                    StartCoroutine(ExecuteAfterTime(2));
                }
            else
            {

            }
                         
        }
   }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        myTransform.transform.parent = Player.transform;
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
}

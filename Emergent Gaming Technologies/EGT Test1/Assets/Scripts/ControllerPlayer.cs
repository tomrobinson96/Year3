using UnityEngine;
using Leap;

/* This script is designed to control all of the player movements, the
   commented out sections were used for the traditional inputs.*/


[AddComponentMenu("Camera-Control/Mouse Look")]
public class ControllerPlayer : MonoBehaviour
{




    //Variables
    /*public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;*/

    public int currentWeapon;
    public GameObject[] weapons;
    private int nrWeapons;

    Controller m_leapController;
    Controller leapController;

    /*public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;*/

    /*float rotationY = 0F;

    int lastFrameFingerCount = 0;*/
    bool paused = false;
    public Rigidbody rb;

    public Transform playerTransform;
    public Transform keyTransform;
    public GameObject player;
    public Rigidbody keyRB;
    GameObject key;
    static int extendedFingers;
    bool pickUp;

    HandList Hands = new HandList();

    void Awake()
    {
        key = GameObject.FindGameObjectWithTag("Grabable");
    }

    void Start() {
        nrWeapons = weapons.Length;        
        SwitchWeapon(currentWeapon);
        m_leapController = new Controller();

        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }


    void Update()
    {
        /*
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }

        CharacterController controller = GetComponent<CharacterController>();
        // is the controller on the ground?
        if (controller.isGrounded)
        {
            //Feed moveDirection with input.
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            //Multiply it by speed.
            moveDirection *= speed;
            //Jumping
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        //Applying gravity to the controller
        moveDirection.y -= gravity * Time.deltaTime;
        //Making the character move
        controller.Move(moveDirection * Time.deltaTime); */




        /*This foreach loop is used to control the weapon that is being
        selected by the user, this is dependnt on how many fingers that 
        are extended on the left hand................................*/ 

        Frame frame = m_leapController.Frame();
        HandList hands = frame.Hands;

        foreach (Hand hand in hands)
        {
            if (hand.IsLeft)
            {
                extendedFingers = 0;
                for (int f = 0; f < hand.Fingers.Count; f++)
                {
                    Finger digit = hand.Fingers[f];
                    if (digit.IsExtended)
                        extendedFingers++;
                }

                if (extendedFingers == 1)
                {
                    weapons[0].gameObject.SetActive(true);
                    weapons[1].gameObject.SetActive(false);
                    weapons[2].gameObject.SetActive(false);
                }
                if (extendedFingers == 2)
                {
                    weapons[0].gameObject.SetActive(false);
                    weapons[1].gameObject.SetActive(true);
                    weapons[2].gameObject.SetActive(false);
                }
                if (extendedFingers == 3)
                {
                    weapons[0].gameObject.SetActive(false);
                    weapons[1].gameObject.SetActive(false);
                    weapons[2].gameObject.SetActive(true);
                }
            }
        }

        for (int i = 1; i <= nrWeapons; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                currentWeapon = i - 1;

                SwitchWeapon(currentWeapon);
            }
        }
        

    }     
     
    void FixedUpdate()
    {
        Hand foremostHand = GetForeMostHand();
        Hand backmostHand = GetBackMostHand();
        if (foremostHand != null)
        {
            MoveCharacter(backmostHand);            
        }
        if (backmostHand != null)
        {
            Debug.Log("backHand");
            ProcessLook(foremostHand);            
        }

        
    }

    /* These two functions set what hand is closest to the screen 
       and which is furthest back, this is used in later functions*/

    Hand GetForeMostHand()
    {
        Frame f = m_leapController.Frame();
        Hand foremostHand = null;
        float zMax = -float.MaxValue;
        for (int i = 0; i < f.Hands.Count; ++i)
        {
            float palmZ = f.Hands[i].PalmPosition.ToUnityScaled().z;
            if (palmZ > zMax)
            {
                zMax = palmZ;
                foremostHand = f.Hands[i];
            }
        }
        return foremostHand;
    }

    Hand GetBackMostHand()
    {
        Frame f = m_leapController.Frame();
        Hand backmostHand = null;
        float zMin = -float.MinValue;
        for (int i = 1; i < f.Hands.Count; ++i)
        {
            float palmZ = f.Hands[i].PalmPosition.ToUnityScaled().z;
            if (palmZ < zMin)
            {
                zMin = palmZ;
                backmostHand = f.Hands[i];
            }
        }
        return backmostHand;
    }


    /* This function is used to control the movement of the camera which is
       acting at the players head, it is locked to moving the X axis as there
       is no need to look up and down for this game*/

        void ProcessLook(Hand hand)
    {
       if (hand.IsRight)
        {
            Debug.Log("LOOK");
           float handX = hand.PalmPosition.ToUnityScaled().x;
           transform.Rotate(Vector3.up, handX * 20.0f);
            
        }
    }

    /* This function is used to control the movement of the player in the scene,
       it is locked to moving forward only as the movements side to side could be
       too comlicated for the used and the movement works just as well this way*/

    void MoveCharacter(Hand hand)
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (hand.IsLeft)
        {
            Debug.Log("Move");
            if (hand.PalmPosition.ToUnityScaled().z > 0)
            {
                Debug.Log("Forward");
                //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                controller.Move(transform.forward * 0.1f);
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                //rb.MovePosition( rb.position += transform.forward * 0.1f);
            }

            if (hand.PalmPosition.ToUnityScaled().z < -1.0f)
            {
                transform.position -= transform.forward * 0.04f;
            }
        }
    }

    void OnGUI()
    {
        if (paused)
        {
            GUILayout.Label("Game is paused!");
            if (GUILayout.Button("Click me to unpause"))
                paused = togglePause();
        }

    }

    //Pauses the game when the user takes their hands out of the scope of the leap motion
    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
    // Old switch weapon function
    void SwitchWeapon(int index)
    {
        
        for (int i = 0; i < nrWeapons; i++)
        {
            if (i == index)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
  }

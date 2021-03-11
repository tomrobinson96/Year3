using UnityEngine;
using System.Collections;

public class BasicPlayer : MonoBehaviour {

    //Move Variables
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    //Look Variables
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    //Game Objects for Questions
    public GameObject empty;
    public GameObject sword;
    public GameObject longSword;
    public GameObject gun;
    public GameObject helloWorld;
    public GameObject welcomeMessage;
    public GameObject multiChoice;
    public GameObject canvas;
    public GameObject question;
    public GameObject next1;
    public GameObject continue1;
    public GameObject next2;
    public GameObject continue2;
    public GameObject info;
    public GameObject mission1;
    public GameObject mission2;
    public GameObject mission3;

    //Hello World
    public GameObject startHW;
    public GameObject dropBoxesHW;

    //Welcome Message
    public GameObject startWM;
    public GameObject infoWM;
    public GameObject dropBoxesWM;

    //Questionnaire
    public GameObject questionStart;
    public GameObject questionInfo;
    public GameObject questionBoxes;
    public GameObject pauseMenu;



    void Update()
    {
        //Pause function
        if (Input.GetButtonDown("Cancel"))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        //Look function
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

        //Move function
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
        controller.Move(moveDirection * Time.deltaTime);
    }


    //Methods bringing up visual studio .exe files
    public void PerformBuild()
    {
        System.Diagnostics.Process.Start("C:/Users/Tom/Documents/Year 3/Final Project/HelloWorldTest/HelloWorldTest/bin/Release/HelloWorldTest.exe");
    }
    public void PerformBuild2()
    {
        System.Diagnostics.Process.Start("C:/Users/Tom/Documents/Year 3/Final Project/WelcomeMessageTest/WelcomeMessageTest/bin/Release/WelcomeMessageTest.exe");
    }
    public void PerformBuild3()
    {
        System.Diagnostics.Process.Start("C:/Users/Tom/Documents/Year 3/Final Project/Questionnaire/Questionnaire/bin/Debug/Questionnaire.exe");
    }


    // ************ The following methods control the displaing of canvas' during the game *************** \\
    public void Next()
    {
        next1.SetActive(true);
        continue1.SetActive(false);
    }

    public void InfoLoad()
    {
        info.SetActive(true);        
    }

    public void Next2()
    {
        next2.SetActive(true);
        continue2.SetActive(false);        
    }
    public void HWBack1()
    {
        info.SetActive(false);
        startHW.SetActive(true);
    }
    public void HWBack2()
    {
        dropBoxesHW.SetActive(false);
        startHW.SetActive(true);
    }
    public void WMBack1()
    {
        infoWM.SetActive(false);
        startWM.SetActive(true);
    }
    public void WMBack2()
    {
        dropBoxesWM.SetActive(false);
        startWM.SetActive(true);
    }
    public void QsBack1()
    {
        questionInfo.SetActive(false);
        questionStart.SetActive(true);
    }
    public void QsBack2()
    {
        questionBoxes.SetActive(false);
        questionStart.SetActive(true);
    }
}

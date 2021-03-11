using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;

///////*  Controls the shooting for the user *////////

public class Shooter : MonoBehaviour {

    Controller controller;
    public Rigidbody projectile;
    public float speed = 20;
    public float timeBetweenShots;
    float timer;
    bool IsRight;

    void Start()
    {
        controller = new Controller();

        //enable the gestures that will be used
        controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
        controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
        controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
        controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;

        
            foreach (Hand hand in hands)
            {
            if (hand.IsRight)
            {
                int extendedFingers = 0;
                for (int f = 0; f < hand.Fingers.Count; f++)
                {
                    Finger digit = hand.Fingers[f];
                    if (digit.IsExtended)
                        extendedFingers++;
                }

                if (extendedFingers == 1 && timeBetweenShots < timer)
                {
                    Debug.Log("shoot");                    
                    Shoot();
                }
            }
        }        
    }

    void Shoot()
    {
        {
            Rigidbody instantiatedProjectile = Instantiate(projectile, transform.position, transform.rotation) as Rigidbody;
            instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
            timer = 0;            
        }
        
    }
}

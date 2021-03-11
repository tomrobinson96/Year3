using UnityEngine;
using Leap;
using UnityEngine.SceneManagement;
using System.Collections;

//////////* Lets the user go back to the home screen when they have reached the winning screen *///////////

public class WinHome : MonoBehaviour {

    Controller controller;

    void Start()
    {
        controller = new Controller();
    }

    void Update()
    {

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

                if (extendedFingers == 5)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}

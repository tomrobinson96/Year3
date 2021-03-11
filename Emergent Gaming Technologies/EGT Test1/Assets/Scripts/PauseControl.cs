using UnityEngine;
using Leap;
using System.Collections;

/* Pause controller*/

public class PauseControl : MonoBehaviour {

    bool paused = false;
    Controller controller;

    void Start()
    {
        controller = new Controller();
    }

        void Update()
    {
        Frame frame = controller.Frame();
        HandList hands = frame.Hands;
        
            if ((hands.Count == 0) && (paused== false))
             {
            paused = true;
            Time.timeScale = 0f;
        }   
             
             else if ((hands.Count != 0))
             {
            paused = false;
            Time.timeScale = 1f;
        }


    }

    void OnGUI()
    {
        if (paused)
        {
            GUILayout.Label("Game is paused!");
            if (GUILayout.Button("Place hands in scope of controller to unpause"));
                //paused = togglePause();
        }
    }

    void togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}


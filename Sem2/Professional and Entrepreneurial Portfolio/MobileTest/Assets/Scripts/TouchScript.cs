using UnityEngine;
using System.Collections;

public class TouchScript : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void OnGUI()
    {

        if (GUI.Button(new Rect(0,0,100,100), "Touch"))
        {
            GetComponent<Camera>().backgroundColor = Color.red;            
        }

        /*foreach (Touch touch in Input.touches)
        {
            string message = "";
            message += "ID: " + touch.fingerId + "\n";
            message += "Phase: " + touch.phase.ToString() + "\n";
            message += "TapCount: " + touch.tapCount + "\n";
            message += "PosX: " + touch.position.x + "\n";
            message += "PosY: " + touch.position.y + "\n";

            int num = touch.fingerId;
            GUI.Label(new Rect(0 + 130 * num, 0, 120, 100), message);
        }*/
    }
}

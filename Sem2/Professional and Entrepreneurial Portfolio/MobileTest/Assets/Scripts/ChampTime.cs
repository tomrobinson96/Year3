using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ChampTime : MonoBehaviour {


    float time;

    public Text timeText;
    public Text timeText1;
    public Text timeText2;
    public Text timeText3;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        setTimeText();
        setTimeText2();
        setTimeText3();
        setTimeText4();
        time += Time.deltaTime;
    }

    void setTimeText()
    {
        timeText1.text = "Time Lasted: " + time.ToString();
    }
    void setTimeText2()
    {
        timeText2.text = "Time Achieved: " + time.ToString();
    }
    void setTimeText3()
    {
        timeText.text = "Time Acheived: " + time.ToString();
    }
    void setTimeText4()
    {
        timeText3.text = "Time Achieved: " + time.ToString();
    }

}

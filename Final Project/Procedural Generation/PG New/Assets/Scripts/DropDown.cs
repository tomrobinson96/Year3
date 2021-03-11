using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DropDown : MonoBehaviour {

    /* This script handles the drop down task within the Questionnaire question */

    public Dropdown myDropdown;
    public GameObject continue1;

    bool answer1;
    bool answer2;
    bool answer3;
    bool answer4;
    bool first2;
    bool second2;

    // Timer to activate the continue button if the player gets stuck
    public GameObject timerStart;
    // Set at 40 seconds
    float timeLeft1 = 40.0f;

    void Start()
    {
        myDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(myDropdown);
        });
    }
    void Destroy()
    {
        myDropdown.onValueChanged.RemoveAllListeners();
    }
    
    /************** Methods that define what the correct answer is and if selected set the boolean to true *************/
    public void myDropdownValueChangedHandler(Dropdown target)
    {       
        if(target.value == 1)
        {           
          answer1 = true;        
        }
    }
    public void myDropdownValueChangedHandler1(Dropdown target)
    {
        if (target.value == 2)
        {   
            answer2 = true;
        }
    }
    public void myDropdownValueChangedHandler2(Dropdown target)
    {
        if (target.value == 1)
        {
            answer3 = true;
        }
    }
    public void myDropdownValueChangedHandler3(Dropdown target)
    {
        if (target.value == 1)
        {           
            answer4 = true;
            continue1.SetActive(true);
        }
    }
    /********** END **********/

    void Update()
    {
        
        if (answer1 == true && answer2 == true && answer3 == true && answer4 == true)
        {
            continue1.SetActive(true);
        }

        if (timerStart.activeInHierarchy)
        {
            timeLeft1 -= Time.deltaTime;
        }
        if (timeLeft1 < 0)
        {
            continue1.SetActive(true);
        }
    }

    public void SetDropdownIndex(int index)
    {
        myDropdown.value = index;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slots : MonoBehaviour, IDropHandler {

    /* This script handles the dropping of options from 'Drag and Drop' questions, 
       leading it to handle the events that happen if the correct answers are in 
       the correct slots */

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    // Reference to if it is Hello World or Welcome Message
    public GameObject lv1;
    public GameObject lv2;

    // Reference to options and slots
    public GameObject ans1;
    public GameObject ans2;
    public GameObject ans3;
    public GameObject ans4;
    public GameObject ans5;
    public GameObject ans6;
    public GameObject ans7;
    public GameObject ans8;
    public GameObject ans9;
    public GameObject ans10;
    public GameObject ans11;
    public GameObject ans12;
    public GameObject ans13;
    public GameObject op1;
    public GameObject op2;
    public GameObject op3;
    public GameObject op4;
    public GameObject op5;
    public GameObject op6;
    public GameObject op7;
    public GameObject op8;
    public GameObject op9;
    public GameObject op10;
    public GameObject op11;
    public GameObject op12;
    public GameObject op13;
    public GameObject cont;
    public GameObject cont1;
    public GameObject timerStart;

    float timeLeft1 = 60.0f;    


    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);            
        }
    }

    void Update()
    {
        if (lv1.activeInHierarchy == true)
        {
            Continue1();
        }
        if (lv2.activeInHierarchy == true)
        {
            Continue2();
        }
        
    }
    private void Continue1()
    {
        if (op1.transform.IsChildOf(ans1.transform) && op2.transform.IsChildOf(ans2.transform) && op3.transform.IsChildOf(ans3.transform) && op4.transform.IsChildOf(ans4.transform) && op5.transform.IsChildOf(ans5.transform))
        {
            Debug.Log("Active");
            cont.SetActive(true);
        }
    }
    private void Continue2()
    {
        if (timerStart.activeInHierarchy)
        {
            Debug.Log("timer start");
            timeLeft1 -= Time.deltaTime;
        }
        if (timeLeft1 < 0)
        {
            Debug.Log("timer end");
            cont1.SetActive(true);
        }

        if (op1.transform.IsChildOf(ans1.transform) && op2.transform.IsChildOf(ans2.transform) && op3.transform.IsChildOf(ans3.transform) && op4.transform.IsChildOf(ans4.transform) && op5.transform.IsChildOf(ans5.transform) && op6.transform.IsChildOf(ans6.transform) && op7.transform.IsChildOf(ans7.transform) && op8.transform.IsChildOf(ans8.transform) && op9.transform.IsChildOf(ans9.transform) && op10.transform.IsChildOf(ans10.transform) && op11.transform.IsChildOf(ans11.transform) && op12.transform.IsChildOf(ans12.transform) && op13.transform.IsChildOf(ans13.transform))
        {
            Debug.Log("Active");
            cont1.SetActive(true);
        }

    }
}

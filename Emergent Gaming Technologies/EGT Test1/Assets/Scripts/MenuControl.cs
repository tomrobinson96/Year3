using UnityEngine;
using System.Collections;

//////////////////* Old Menu script */////////////


/*
public class MenuControl : MonoBehaviour
{
    private float clickableElapsedTime;

    public Vector3 CurrMousePos { get; private set; }
    public object buttonHover { get; private set; }

    void Update()
    {
        // do a test ray to see if we are on top of a clickable
        Ray testRay = Camera.main.ScreenPointToRay(CurrMousePos);
        if (Physics.Raycast(testRay, out hit))
        {
            if (hit.collider.gameObject.GetComponent())
            {
                // The below is specific to my game.
                // But basically need to get a unique string for what you are hovering on top of
                buttonHover = hit.collider.gameObject.GetComponent();
                string clickablebutton = buttonHover.id + buttonHover.who + buttonHover.idLevel.ToString();
                // MouseOverClickable keeps track of whether we are on top of the same button
                // as well as starting the timer to see if a hoverclick is initiated.
                MouseOverClickable(clickablebutton);
            }
            else
            {
                // we are not on top off a button, so still call MouseOverClickable, but with an ever changing ID
                MouseOverClickable("");
                clickableElapsedTime = 0F;
            }
        }
        else
        {
            MouseOverClickable("");
            clickableElapsedTime = 0F;
        }
    }

    public void MouseOverClickable(string ClickableID)
    {
        if (ClickableID == "")
        {
            lastClickableID = ClickableID;
            hoverClick = false;
            clickableElapsedTime = 0F;
        }
        else if (ClickableID == lastClickableID)
        {
            clickableElapsedTime = clickableElapsedTime + Time.deltaTime;
            if (clickableElapsedTime & gt; 1)
		{
                // elapsed time is greater than 1 second, so trigger a hoverClick
                hoverClick = true;
                clickableElapsedTime = 0F;
            }
        }
        else
        {
            lastClickableID = ClickableID;
            hoverClick = false;
            clickableElapsedTime = 0F;
        }
    }

    void OnGUI()
    {
        // so we only need to show the "timer glow" if we have the conditions of:
        if (clickableElapsedTime & gt; 0) 
	{
            // Work out which if 12 bitmaps to use (showing progression clockwise)
            int glowTimerIndex = Mathf.CeilToInt(11 * clickableElapsedTime);
            if (glowTimerIndex & gt; 11)
		{
                glowTimerIndex = 11;
            }
            Texture2D cursorGlowImg = CursorGlowArray[glowTimerIndex];
            graphicRect = new Rect(x - (cursorGlowImg.width / 2), y - (cursorGlowImg.height / 2), cursorGlowImg.width, cursorGlowImg.height); ;
            GUI.DrawTexture(graphicRect, cursorGlowImg);
        }
    }
}*/

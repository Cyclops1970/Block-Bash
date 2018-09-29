using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBounds : MonoBehaviour
{

    public RectTransform cont;
    public GameObject button;

    float buttonSize;
    int numberOfButtons;

    float xPos, yPos;
    Vector2 scrollPosition;

    /* needs to be updated to be exact, it gets out of sync with larger number of levels*/

    private void Start()
    {
        buttonSize = 132;
        //buttonSize = 125f;
        //
        numberOfButtons = GameManager.manager.levelCount;

        //Set scroll back to where you left it.
        xPos = PlayerPrefs.GetFloat("scrollPosition.x");
        yPos = PlayerPrefs.GetFloat("scrollPosition.y");
        scrollPosition = new Vector2(xPos, yPos);
        cont.offsetMax = scrollPosition;

    }

    void Update()
    {
        if (cont.offsetMax.y < 0)
        { 
            cont.offsetMax = new Vector2(); //Sets its value back.
            cont.offsetMin = new Vector2(); //Sets its value back.

        }

        if (cont.offsetMax.y > (numberOfButtons * buttonSize) - buttonSize*10)
        { 
            cont.offsetMax = new Vector2(0, (numberOfButtons * buttonSize) - buttonSize*10); // Set its value back
            cont.offsetMin = new Vector2(); //Depending on what values you set on your scrollview, you might want to change this, but my one didn't need it.
        }

        //Store the cont.offset so that when you go back to the home screen you are at the same spot.
        GameManager.manager.scrollPosition = cont.offsetMax;
        PlayerPrefs.SetFloat("scrollPosition.x", cont.offsetMax.x);
        PlayerPrefs.SetFloat("scrollPosition.y", cont.offsetMax.y);
    }
    /*
     * When dynamically adding UI elements to a scrollable content Panel, a ContentSizeFitter component is of great help. 
     * It will make the scrollable area size “snap” to the combined area of the internal UI elements. For this to work:
     * 1) the children UI elements each need a LayoutElement with set preferred sizes (width, height, or both) and 2) 
     * the panel with the ContentSizeFitter needs its layout group’s Child Force Expand (width, height, or both) setting(s) disabled.
     */
}

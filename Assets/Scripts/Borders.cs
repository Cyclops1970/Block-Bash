using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour {

    public GameManager m;

    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

	// Use this for initialization
	void Start ()
    {
        // These are 1x1 scale, 
        // Left and right are fine, showing at edge of screen
        // top and bottom need to change position to a % of screen

        //scale width of screen and freeTopArea for height size
        top.transform.localScale = new Vector2(m.camX, m.camY*m.freeTopArea);
        top.transform.localPosition = new Vector2(0, m.camY/2-top.transform.localScale.y/2);
        //same for bottom
        bottom.transform.localScale = new Vector2(m.camX, m.camY*m.freeBottomArea);
        bottom.transform.localPosition = new Vector2(0, -m.camY/2+bottom.transform.localScale.y/2);


        //this puts them just off screen
        left.transform.localPosition = new Vector2(-m.camX /2-0.5f, 0);
        left.transform.localScale = new Vector2(left.transform.localScale.x, m.camY);
        right.transform.localPosition = new Vector2(m.camX /2+0.5f, 0);
        right.transform.localScale = new Vector2(right.transform.localScale.x, m.camY);


    }


}

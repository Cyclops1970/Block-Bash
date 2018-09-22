using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateLevels : MonoBehaviour {

    public GameObject content; // This will be the parent for all the level buttons
    public Sprite zeroStar, oneStar, twoStar, threeStar;

	// Use this for initialization
	void Start ()
    {
        DisplayLevels();

	}

    void DisplayLevels()
    {
        GameObject level;
        Image starSprite;
        int stars;

        for(int x = 1; x <= GameManager.manager.levelCount; x++)
        {
            //get stars of level x
            stars = PlayerPrefs.GetInt("level" + x + "stars");

            if (x<=PlayerPrefs.GetInt("highestLevel") || stars > 0)
            {
                level = Instantiate(GameManager.manager.levelReady);
        
                starSprite = level.gameObject.GetComponentsInChildren<Image>()[1]; //[1] to avoid the image in the parent

                //Show the correct level of stars for this level
                if (stars == 1)
                    starSprite.sprite = oneStar;
                else if (stars == 2)
                    starSprite.sprite = twoStar;
                else if (stars == 3)  //GameManager.manager.level[x].stars == 3
                    starSprite.sprite = threeStar;
                else
                    starSprite.sprite = zeroStar;
                    
            }
            else
            {
                level = Instantiate(GameManager.manager.levelLocked);
            }

            level.transform.SetParent(content.transform);
            level.transform.localScale = new Vector3(1, 1, 1);

            level.GetComponentInChildren<Text>().text = (x).ToString(); 

        }
    }

}

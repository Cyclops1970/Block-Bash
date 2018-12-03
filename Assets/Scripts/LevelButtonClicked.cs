using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class LevelButtonClicked : MonoBehaviour
{
    string gameID = "2945641";
    bool testMode = false;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("ads", 0) == 0) //default value returned (0) so ads are still valid
        {
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(gameID, testMode);  //// 1st parameter is String and 2nd is boolean
            }
        }
    }

    public void PlayGame()
    {
        
        //Set the current level being played
        GameManager.manager.currentLevel = int.Parse(gameObject.GetComponentInChildren<Text>().text);

        //Set the number of level retries to zero
        GameManager.manager.retries = 0;

        //update the number of levels played
        GameManager.manager.levelsPicked++;
        
        if(GameManager.manager.levelsPicked>=4)
        {
            StartCoroutine(PlayAd());
        }
        else
        {
            //Load the PlayGame Scene
            SceneManager.LoadScene("PlayGame");
        }
    }

    //Play Ad
    IEnumerator PlayAd()
    {
        if (PlayerPrefs.GetInt("ads",0) == 0) //default value returned (0) so ads are still valid (set to 1 when purchased)
        {
            // play an ad
            if (Advertisement.IsReady("video"))
            {
                //print("Ad is ready!");
                Advertisement.Show("video");
            }

            while (Advertisement.isShowing)
            {
                //print("showing ad");
                yield return null;
            }
        }
        //reset levelsPlayed
        GameManager.manager.levelsPicked = 0;

        //Load the PlayGame Scene
        SceneManager.LoadScene("PlayGame");
        yield return null;
    }
}
	


using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class NextLevel: MonoBehaviour
{
    public GameObject endOfLevelsPanel, passPanel;
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
    public void PlayNextLevel()
    {
        //Set the current level being played
        GameManager.manager.currentLevel++;
        //reset level retires 
        GameManager.manager.retries = 0;
        //increase number of levels played
        GameManager.manager.levelsPlayed++;

        if (GameManager.manager.currentLevel > GameManager.manager.levelCount)
        {
            passPanel.SetActive(false);
            EndOfLevels();

        }
        else
        {
            if(GameManager.manager.levelsPlayed == 3)
            {
                StartCoroutine(PlayAd());
            }
            else
            {
                //Load the PlayGame Scene
                SceneManager.LoadScene("PlayGame");
            }
        }
    }

    //Play Ad
    IEnumerator PlayAd()
    {

        if (PlayerPrefs.GetInt("ads", 0) == 0) //default value returned (0) so ads are still valid
        {   // play an ad
            if (Advertisement.IsReady("video"))
            {
                Advertisement.Show("video");
            }

            while (Advertisement.isShowing)
            {
                yield return null;
            }
        }
        //reset levelsPlayed
        GameManager.manager.levelsPlayed = 0;

        //Load the PlayGame Scene
        SceneManager.LoadScene("PlayGame");
        yield return null;
    }


    void EndOfLevels()
    {
        endOfLevelsPanel.SetActive(true);
    }

    public void ContinueButton()
    {
        endOfLevelsPanel.SetActive(false);
        //Load the PlayGame Scene
        SceneManager.LoadScene("Home");
    }
}
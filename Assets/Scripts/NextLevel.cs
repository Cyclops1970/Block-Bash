using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel: MonoBehaviour
{
    public GameObject endOfLevelsPanel, passPanel;

    public void PlayNextLevel()
    {
        //Set the current level being played
        GameManager.manager.currentLevel++;
        //reset level retires 
        GameManager.manager.retries = 0;

        if (GameManager.manager.currentLevel > GameManager.manager.levelCount)
        {
            passPanel.SetActive(false);
            EndOfLevels();

        }
        else
        {
            //Load the PlayGame Scene
            SceneManager.LoadScene("PlayGame");
        }
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
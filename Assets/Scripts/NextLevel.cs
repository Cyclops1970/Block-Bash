using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel: MonoBehaviour
{
    public void PlayNextLevel()
    {
        //Set the current level being played
        GameManager.manager.currentLevel++;

        //Load the PlayGame Scene
        SceneManager.LoadScene("PlayGame");

        //reset level retires 
        GameManager.manager.retries = 0;

    }
}
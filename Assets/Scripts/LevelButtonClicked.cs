using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonClicked : MonoBehaviour
{
    public void PlayGame()
    {
        //Set the current level being played
        GameManager.manager.currentLevel = int.Parse(gameObject.GetComponentInChildren<Text>().text);

        //Set the number of level retries to zero
        GameManager.manager.retries = 0;

        //Load the PlayGame Scene
        SceneManager.LoadScene("PlayGame");

    }
}
	


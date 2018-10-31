using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassPanel : MonoBehaviour
{
    public PlayLevel playLevel;

    public GameObject neutralFace, smileFace, happyFace;
    public GameObject newHighScore, newLowestShots;

    public TextMeshProUGUI levelCoinText;
    public TextMeshProUGUI newHighScoreCoinText;
    public TextMeshProUGUI newLowestShotsCoinText;
    public GameObject noNewHighScore, noNewLowestShots;
    public GameObject bottomPanel;

    Color32 grey = new Color32(90, 90, 90, 255);
    Color32 green = new Color32(0, 255, 0, 255);

    private void OnEnable()
    {
        happyFace.GetComponent<Image>().color = grey;
        smileFace.GetComponent<Image>().color = grey;
        neutralFace.GetComponent<Image>().color = grey;

        //Text for Level Completed coins
        if (GameManager.manager.currentLevelStars == 3)
        {
            happyFace.GetComponent<Image>().color = green;
            smileFace.GetComponent<Image>().color = green;
            neutralFace.GetComponent<Image>().color = green;

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                levelCoinText.text = (GameManager.manager.starCoins3 * 2).ToString();
            else
                levelCoinText.text = GameManager.manager.starCoins3.ToString();
        }
        else if (GameManager.manager.currentLevelStars == 2)
        {
            smileFace.GetComponent<Image>().color = green;
            neutralFace.GetComponent<Image>().color = green;

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                levelCoinText.text = (GameManager.manager.starCoins2 * 2).ToString();
            else
                levelCoinText.text = GameManager.manager.starCoins2.ToString();
        }
        else
        {
            neutralFace.GetComponent<Image>().color = green;

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                levelCoinText.text = (GameManager.manager.starCoins1 * 2).ToString();
            else
                levelCoinText.text = GameManager.manager.starCoins1.ToString();

        }

        //Text for new high score
        if(GameManager.manager.newHighScore == true)
        {
            newHighScore.SetActive(true);
            noNewHighScore.SetActive(false);

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                newHighScoreCoinText.text = (GameManager.manager.newHighScoreCoins * 2).ToString();
            else
                newHighScoreCoinText.text = GameManager.manager.newHighScoreCoins.ToString();
        }
        else
        {
            newHighScore.SetActive(false);
            noNewHighScore.SetActive(true);
        }

        //Text for new lowest Shots
        if (GameManager.manager.newLowestShots == true)
        {
            newLowestShots.SetActive(true);
            noNewLowestShots.SetActive(false);

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                newLowestShotsCoinText.text = (GameManager.manager.newLowestShotsCoins * 2).ToString();
            else
                newLowestShotsCoinText.text = GameManager.manager.newLowestShotsCoins.ToString();
        }
        else
        {
            newLowestShots.SetActive(false);
            noNewLowestShots.SetActive(true);
        }

        //Disable the bottom panel
        bottomPanel.SetActive(false);
    }
}







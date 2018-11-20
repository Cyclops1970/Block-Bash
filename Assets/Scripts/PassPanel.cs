using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassPanel : MonoBehaviour
{
    public PlayLevel playLevel;

    public GameObject neutralFace, smileFace, happyFace, faces;
    public GameObject newHighScore, newLowestShots, levelComplete;

    public TextMeshProUGUI levelCoinText;
    public TextMeshProUGUI newHighScoreCoinText;
    public TextMeshProUGUI newLowestShotsCoinText;
    public GameObject noNewHighScore, noNewLowestShots;
    public GameObject bottomPanel, shotPanel;

    Color32 grey = new Color32(90, 90, 90, 255);
    Color32 green = new Color32(0, 255, 0, 255);

    private void OnEnable()
    {
        Time.timeScale = 1;

        //StartCoroutine(PanelEntry());

        happyFace.GetComponent<Image>().color = Color.clear;
        smileFace.GetComponent<Image>().color = Color.clear;
        neutralFace.GetComponent<Image>().color = Color.clear; //grey

        //Text for Level Completed coins
        if (GameManager.manager.currentLevelStars == 3)
        {
            AudioSource.PlayClipAtPoint(GameManager.manager.levelPassSoundLaughing, Vector3.zero);

            StartCoroutine(FaceAnim());
            happyFace.GetComponent<Image>().color = green;
            //smileFace.GetComponent<Image>().color = green;
            //neutralFace.GetComponent<Image>().color = green;

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
            {
                levelCoinText.text = (GameManager.manager.starCoins3 * 2).ToString();
            }
            else
            {
                levelCoinText.text = GameManager.manager.starCoins3.ToString();
            }
                
            
        }
        else if (GameManager.manager.currentLevelStars == 2)
        {
            StartCoroutine(FaceAnim());
            smileFace.GetComponent<Image>().color = green;
            //neutralFace.GetComponent<Image>().color = green;

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
            {
                levelCoinText.text = (GameManager.manager.starCoins2 * 2).ToString();
            }
            else
            {
                levelCoinText.text = GameManager.manager.starCoins2.ToString();
            }

            AudioSource.PlayClipAtPoint(GameManager.manager.levelPassSoundHappy, Vector3.zero);
        }
        else
        {
            neutralFace.GetComponent<Image>().color = green;

            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
            {
                levelCoinText.text = (GameManager.manager.starCoins1 * 2).ToString();
            }
            else
            {
                levelCoinText.text = GameManager.manager.starCoins1.ToString();
            }

            AudioSource.PlayClipAtPoint(GameManager.manager.levelPassSound, Vector3.zero);
        }
        //StartCoroutine(ScoreAnim(levelComplete,0));
        //Text for new high score
        if(GameManager.manager.newHighScore == true)
        {
            newHighScore.SetActive(true);
            noNewHighScore.SetActive(false);
            //StartCoroutine(ScoreAnim(newHighScore,0.25f));

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
            //StartCoroutine(ScoreAnim(newLowestShots, 0.5f));

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
        shotPanel.SetActive(false);
    }

    /*
    IEnumerator ScoreAnim(GameObject thing, float wait)
    {
        float elapsedTime = 0, currentTimeScale = Time.timeScale, displayTime = .751f;
        Vector2 startingScale = new Vector2(9,19);
        Vector2 endingScale = new Vector2(1, 1);// thing.transform.localScale;

        yield return new WaitForSeconds(wait);

        while (elapsedTime < displayTime)//*currentTimeScale)
        {
            thing.transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / displayTime));
            elapsedTime += Time.deltaTime / currentTimeScale;

            yield return null;
        }
        thing.transform.localScale = endingScale;

        yield return null;
    }
    */

    IEnumerator FaceAnim()
    {
        float lerpTime = 0.75f;
        float elapsedTime = 0, rotTime = 0;
        Vector2 startingScale = faces.transform.localScale * 1f;
        Vector2 endingScale = startingScale * 1.5f;
        Vector2 tempScale;
        Quaternion startRot, endRot, tempRot, currentRotation;

        startRot = Quaternion.Euler(new Vector3(0, 0, 10));
        endRot = Quaternion.Euler(new Vector3(0, 0, -10));

        while (this.enabled==true)
        {
            while (elapsedTime < lerpTime)
            {
                if (GameManager.manager.currentLevelStars == 3)
                {
                    faces.transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / lerpTime));
                    elapsedTime += Time.deltaTime;
                }

                faces.transform.localRotation = Quaternion.Lerp(startRot, endRot, (rotTime*3 / lerpTime));
                rotTime += Time.deltaTime;
                if (rotTime*3 > lerpTime)
                {
                    rotTime = 0;
                    tempRot = startRot;
                    startRot = endRot;
                    endRot = tempRot;
                }
                yield return null;
            }
            //reset and go the other way
            elapsedTime = 0;
            tempScale = startingScale;
            startingScale = endingScale;
            endingScale = tempScale;

            Time.timeScale = 1;

            yield return null;
        }
        yield return null;
    }
    /*
    IEnumerator PanelEntry()
    {
        RectTransform panel;
        float panelHeight;

        panel = GetComponent<RectTransform>();
        panelHeight = panel.rect.height;

        yield return null;
    }
    */
}







using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RedoShot : MonoBehaviour
{
    public BallControl ballControl;
    public GameObject ballExplosion;
    public PlayLevel playLevel;
    public LevelGenerator levelGenerator;
    public TextMeshProUGUI redoCostText;
    public GameObject coinIcon;
    public Button redoButton;

    float timer = 1.5f;

    private void Start()
    {
        redoCostText.text = GameManager.manager.redoShotCost.ToString();
    }

    private void Update()
    {
        if (GameManager.manager.playerCoins >= GameManager.manager.redoShotCost)
        {

            redoButton.interactable = true;
            coinIcon.GetComponent<Image>().color = Color.white;
            redoCostText.color = Color.white;
        }
        else
        {
            redoButton.interactable = false;
            coinIcon.GetComponent<Image>().color = Color.grey;
            redoCostText.color = Color.grey;
        }
    }

    public void RedoWrapper()
    {
        Time.timeScale = 1; // reset time 

        GameManager.manager.playerCoins -= GameManager.manager.redoShotCost;
        PlayerPrefs.SetInt("playerCoints", GameManager.manager.playerCoins);
        
        //set redo to true. used to stop bombs etc.
        GameManager.manager.redo = true;
        StartCoroutine(Redo());
    }

    public IEnumerator Redo()
    {
        //Quaternion resetRotation;

        while (GameManager.manager.keepWaiting == true) //
        {
            print("waiting....");
            yield return null;
        }
        StartCoroutine(GameManager.manager.Message("Undo Shot!", Vector3.zero, 15, timer, Color.white));
        AudioSource.PlayClipAtPoint(GameManager.manager.undoShot, Vector2.zero);

        //Hide the Shot Panel
        StartCoroutine(playLevel.ShotPanelHide());

        //state balls are not active
        GameManager.manager.ballsActive = false;
        //reduce the shots taken
        playLevel.shotsTaken -= 1;

        //destroy balls
        foreach (Ball b in ballControl.balls)
        {
            if (b.ball != null)
            {
                //AudioSource.PlayClipAtPoint(GameManager.manager.ballExplodeSound, b.ball.transform.localPosition);

                Instantiate(ballExplosion, b.ball.transform.localPosition, Quaternion.identity);
                Destroy(b.ball);
            }
        }

        //restore blocks to same state as start of shot
        for (int y = 0; y < levelGenerator.currentLevel.height; y++)
        {
            for (int x = 0; x < levelGenerator.currentLevel.width; x++)
            {
                if (PlayLevel.currentBlockStatus[x, y] != null)
                {
                    //Check if block destroyed or not
                    if (((levelGenerator.block[x, y] == null) || (levelGenerator.block[x, y].GetComponent<Block>().hitsRemaining <= 0)) && (PlayLevel.currentBlockStatus[x, y] != null))
                    {
                        //prevent duplication of bombs,specials etc as they have no hits remaining
                        if (PlayLevel.currentBlockStatus[x, y].tag != "block")
                        {
                            Destroy(levelGenerator.block[x, y].gameObject);
                        }

                        levelGenerator.block[x, y] = PlayLevel.currentBlockStatus[x, y];
                        levelGenerator.block[x, y].gameObject.SetActive(true);
                    }

                    levelGenerator.block[x, y].GetComponent<Block>().hitsRemaining = PlayLevel.currentBlockHitsRemaining[x, y];

                    if (levelGenerator.block[x, y].CompareTag("block") == true)
                    {
                        levelGenerator.block[x, y].GetComponentInChildren<TextMeshProUGUI>().text = levelGenerator.block[x, y].GetComponent<Block>().hitsRemaining.ToString();
                        levelGenerator.block[x, y].gameObject.GetComponent<SpriteRenderer>().color = levelGenerator.block[x, y].gameObject.GetComponentInChildren<Block>().colour;
                    }
                    levelGenerator.block[x, y].gameObject.GetComponentInChildren<Collider2D>().enabled = true;

                    //resetRotation = levelGenerator.block[x, y].gameObject.transform.rotation;
                    StartCoroutine(DisplayUndoneBlock(x, y));
                    //Scale block correctly
                    levelGenerator.block[x, y].GetComponent<Block>().transform.localScale = new Vector2(levelGenerator.blockScaleAdjustedX, levelGenerator.blockScaleAdjustedY);
                    //Reset rotation to ensure looks proper
                    //levelGenerator.block[x, y].GetComponent<Block>().transform.rotation = resetRotation;
                }
            }
        }

        //reset colours
        playLevel.BlockColour();

        //Reward line, highscores etc.
        playLevel.rewardLine.fillAmount = playLevel.rewardLineStartOfShot;
        //update smiley faces
        if (playLevel.rewardLine.fillAmount >= 1)
        {
            playLevel.laughingFace.color = new Color32(255, 255, 255, 255);
            playLevel.happyFace.color = new Color32(255, 255, 255, 255);
            playLevel.neutralFace.color = new Color32(255, 255, 255, 255);
        }
        else if (playLevel.rewardLine.fillAmount >= 0.75)
        {
            playLevel.laughingFace.color = new Color32(128, 128, 128, 255);
            playLevel.happyFace.color = new Color32(255, 255, 255, 255);
            playLevel.neutralFace.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            playLevel.laughingFace.color = new Color32(128, 128, 128, 255);
            playLevel.happyFace.color = new Color32(128, 128, 128, 255);
            if (playLevel.shotsTaken == 0)
            {
                playLevel.neutralFace.color = new Color32(128, 128, 128, 255);
            }
            else
            {
                playLevel.neutralFace.color = new Color32(255, 255, 255, 255);
            }

        }
        //Scores
        if (playLevel.hs == true)
        {
            playLevel.hs = false;
            GameManager.manager.newHighScore = false;
            //GameManager.manager.level[GameManager.manager.currentLevel].highestPoints = playLevel.highScoreStartOfShot;
        }
        GameManager.manager.level[GameManager.manager.currentLevel].highestPoints = playLevel.highScoreStartOfShot;
        //show highest points
        playLevel.highScoreText.text = ("Highest: " + GameManager.manager.level[GameManager.manager.currentLevel].highestPoints);
        
        //set current shot to 0
        GameManager.manager.level[GameManager.manager.currentLevel].shotPoints = 0;
        playLevel.shotScoreText.text = "0";

        yield return null;
    }

    IEnumerator DisplayUndoneBlock(int x, int y)
    {
        float elapsedTime = 0;
        float displayTime = timer;
        Vector2 startingScale = new Vector2(5,5);
        Vector2 endingScale;
        //Vector3 startRot = new Vector3(0, 0, 180);
        //Vector3 endRot = new Vector3(0, 0, 0);
        //Quaternion startQuat, endQuat;

        endingScale = new Vector2(levelGenerator.blockScaleAdjustedX, levelGenerator.blockScaleAdjustedY);
        //startQuat = Quaternion.Euler(startRot);
        //endQuat = Quaternion.Euler(endRot);

        while (elapsedTime < displayTime)//*currentTimeScale)
        {
            //levelGenerator.block[x, y].transform.rotation = Quaternion.Lerp(startQuat, endQuat, (elapsedTime / displayTime));
            levelGenerator.block[x, y].transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / displayTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //Final step to ensure all blocks are correctly scaled again...lerp sometimes leaves out last step??
        levelGenerator.block[x, y].transform.localScale = endingScale;
    }
}

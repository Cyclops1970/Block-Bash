using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class PlayLevel : MonoBehaviour
{
    //[HideInInspector]
    static public PlayLevel playLevel;

    [Header("")]
    //[Header("Game Manager")]
    //public GameManager m;
    public LevelGenerator levelGenerator; //use this type if levelGenerator won't be active when needed it here..nullref fix!!
    public HorizontalBlock horizontalBlock;

    public Balls2x balls2x;
    public Button balls2xButton;
    public BlockReduction blockReduction;
    public Button blockReductionButton;
    public FloorBlock floorBlock;
    public Button FloorBlockButton;
    public InvincibleBalls invincibleBalls;
    public Button invincibleBallsButtton;
    public Button shopButton;

    [Header("Borders")]
    public GameObject top;
    public GameObject bottom;
    public GameObject left;
    public GameObject right;

    [Header("Balls")]
    public BallControl ballControl;
    public GameObject ballExplosion;

    [HideInInspector]
    public Vector3 startPos, endPos;

    [HideInInspector]
    public bool ableToShoot = false;

    [Header("Text Fields and Panels")]
    public TextMeshProUGUI shotScoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI numberOfBallsText;
    public TextMeshProUGUI numberOfBallsInPlayText;
    public TextMeshProUGUI shotsTakenText;
    public TextMeshProUGUI lowestShotsTakenText;
    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI balls2xText;
    public GameObject balls2xCoins;
    public TextMeshProUGUI blockReductionText;
    public GameObject blockReductionCoins;
    public TextMeshProUGUI floorBlockText;
    public GameObject floorBlockCoins;
    public TextMeshProUGUI invincibleBallsText;
    public GameObject invincibleBallsCoins;

    public GameObject passPanel, failPanel, shotPanel, bottomPanel;

    [Header("Reward Line")]
    public Image rewardLine;
    public Image neutralFace;
    public Image happyFace;
    public Image laughingFace;

    //[HideInInspector]
    //public int levelScore;

    [HideInInspector]
    public int shotsTaken;

    bool levelFailed;
    public bool bottomReached;

    //used for reward line  
    static float levelPercentage;
    static float oldFillAmount;

    bool hs = false; // used to determine if new HS message should be played

    Color32 textGreen = new Color32(11, 196, 0, 255);
    
    // Use this for initialization
    void Start ()
    {
        levelFailed = false;
        bottomReached = false;
        GameManager.manager.newHighScore = false;
        GameManager.manager.newLowestShots = false;
        shotsTaken = 0;

        //Get the location of the warning area where blocks turn red
        GameManager.manager.warningArea = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (levelGenerator.blockScaleAdjustedY * 1.5);

        //Hide results panel
        passPanel.SetActive(false);
        failPanel.SetActive(false);

        SetupBorders();

        //Generate the level to be played
        levelGenerator.GenerateLevel();

        StartCoroutine(Play());
    }

    void CopyStuff()
    {
        int element = 0;
        
        foreach(ColourToPrefab c in levelGenerator.colourMappings)
        {
            levelGenerator.level.ctp[element].colour = c.colour;
            levelGenerator.level.ctp[element].blockType = c.blockType;
            levelGenerator.level.ctp[element].hitsToKill = c.hitsToKill;
            element++;
        }
        
    }
    IEnumerator SwapPanels()
    {
        float deathTime = 0.25f;
        float elapsedTime = 0;
        Vector2 startingScale = bottomPanel.transform.localScale;
        Vector2 endingScale = new Vector2(0.001f, 0.001f);

        //if shotpanel already fullsize
        if ((Vector2)shotPanel.transform.localScale == startingScale)
        {
            yield return new WaitForSeconds(0.25f);

            while (elapsedTime < deathTime)
            {
                shotPanel.transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / deathTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            //shotPanel.SetActive(!shotPanel.activeInHierarchy);
        }
        else // is minimised and needs to be expanded
        {
            while (elapsedTime < deathTime)
            {
                shotPanel.transform.localScale = Vector2.Lerp(endingScale, startingScale, (elapsedTime / deathTime));
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            //do this cause it doesn't actually get to the proper size 
            shotPanel.transform.localScale = startingScale;
            //shotPanel.SetActive(!shotPanel.activeInHierarchy);
        }
    
        //shotPanel.SetActive(!shotPanel.activeInHierarchy);
        //bottomPanel.SetActive(!bottomPanel.activeInHierarchy);

    }

    void SetupBorders()
    {
        // These are 1x1 scale, 
        // Left and right are fine, showing at edge of screen
        // top and bottom need to change position to a % of screen

        Instantiate(top);
        Instantiate(bottom);
        Instantiate(left);
        Instantiate(right);

        //THIS NEEDS TO BE FIXED!
        /*
        //scale width of screen and freeTopArea for height size
        top.transform.localScale = new Vector2(m.camX, m.camY * m.freeTopArea);
        top.transform.localPosition = new Vector2(0, m.camY / 2 - top.transform.localScale.y / 2);

        bottom.transform.localScale = new Vector2(m.camX, m.camY * m.freeBottomArea);
        bottom.transform.localPosition = new Vector2(0, -m.camY / 2 + bottom.transform.localScale.y / 2);

        //this puts them just off screen
        left.transform.localPosition = new Vector2(-m.camX / 2 - 0.5f, 0);
        left.transform.localScale = new Vector2(left.transform.localScale.x, m.camY);
        right.transform.localPosition = new Vector2(m.camX / 2 + 0.5f, 0);
        right.transform.localScale = new Vector2(right.transform.localScale.x, m.camY);
        */
    }
	
    public IEnumerator Play()
    {
        //Get current highscore (done so it isn't stored on replays of same level)
        GameManager.manager.level[GameManager.manager.currentLevel].highestPoints = PlayerPrefs.GetInt("level" + GameManager.manager.currentLevel + "highestPoints");

        GameManager.manager.newHighScore = false;
        GameManager.manager.newLowestShots = false;

        GameManager.manager.ballsActive = false;
        ableToShoot = false;
        //int currentPoints = 0;

        //set current shot to 0
        GameManager.manager.level[GameManager.manager.currentLevel].shotPoints = 0;
        //show highest points
        highScoreText.text = ("Highest: " + GameManager.manager.level[GameManager.manager.currentLevel].highestPoints);
        //show lowest shots taken
        lowestShotsTakenText.text = ("Lowest: " + PlayerPrefs.GetInt("level" + GameManager.manager.currentLevel + "lowestShotsTaken"));// GameManager.manager.level[GameManager.manager.currentLevel].lowestShotsTaken);
        //show the level number
        levelNumberText.text = "Level " + GameManager.manager.currentLevel.ToString();
        //show player coins
        playerCoinsText.text = GameManager.manager.playerCoins.ToString();

        //used for the reward line
        rewardLine.fillAmount = 0;
        oldFillAmount = 0;

        //Check if firstRun to stop getting highscores when highest score == 0 (lowestshots checked at end of level)
        if(GameManager.manager.level[GameManager.manager.currentLevel].highestPoints==0)
        {
            GameManager.manager.firstRun = true;
        }
        else
        {
            GameManager.manager.firstRun = false;
        }

        //Set the number of balls according to current level;
        GameManager.manager.maxNumberOfBalls = GameManager.manager.baseNumberOfBalls + (int)(Mathf.Round((GameManager.manager.currentLevel / 5)));

        //Initialise block colours (if not a bonus level) if bonus level, say so :)
        if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel != 0)
            BlockColour();
        else
        {
            StartCoroutine(GameManager.manager.Message("Bonus Level" + "\r\n" + "Double Coins!", new Vector2(0, 0), 8, 1.5f, Color.white));
            BlockColour(); // remove if doing the bonus levels with their own colour
        }
            

        //Play until level end
        while ((GameManager.manager.actualNumberOfBlocks > 0) && (!levelFailed))
        {
            //launch balls
            LaunchBalls();
            
            // Check if all balls have finished on screen and move blocks down
            BallsFinished(); //move blocks down

            //update the level text
            UpdateLevelText();
            
            //update the reward line
            UpdateRewardLine();
            /*
            if(Input.GetKeyDown(KeyCode.T))
            {
                print("Copying to scriptable object.");
                CopyStuff();
            }
            */
            //Check to see if balls are active, if so prevent shop buttons
            /*
            if(GameManager.manager.ballsActive==true)
            {
                shopButton.interactable = false;
                balls2xButton.interactable = false;
                blockReductionButton.interactable = false;
                invincibleBallsButtton.interactable = false;
                FloorBlockButton.interactable = false;
            }
            else
            {
                shopButton.interactable = true;
                balls2xButton.interactable = true;
                blockReductionButton.interactable = true;
                invincibleBallsButtton.interactable = true;
                FloorBlockButton.interactable = true;
            }
            */
            yield return null;

            //Double check all blocks are gone (sometimes there are blocks left in frantic levels)
            GameObject[] block = GameObject.FindGameObjectsWithTag("block");
            GameManager.manager.actualNumberOfBlocks = block.Length;
        }
        
        //Mark level as done and available in future
        if(GameManager.manager.actualNumberOfBlocks <= 0)
        {
            // Remove any errant balls
            StartCoroutine(ForceBallsToFinish());

            //slight delay
            for (int wait = 0; wait < 150; wait++)
            {
                yield return null;
            }

            //if level completed check if lowestshotstaken needs to be updated
            //update lowest shots taken
            if ((shotsTaken < GameManager.manager.level[GameManager.manager.currentLevel].lowestShotsTaken) || (GameManager.manager.level[GameManager.manager.currentLevel].lowestShotsTaken == 0))
            {
                //Avoid giving new record on first run
                if(GameManager.manager.level[GameManager.manager.currentLevel].lowestShotsTaken != 0)
                {
                    //GameManager.manager.level[GameManager.manager.currentLevel].lowestShotsTaken = shotsTaken;
                    GameManager.manager.newLowestShots = true;
                }
                GameManager.manager.level[GameManager.manager.currentLevel].lowestShotsTaken = shotsTaken;
                lowestShotsTakenText.text = ("Lowest: " + shotsTaken);
                
            }

            //set the stars for the level These are the stored stars!
            if (rewardLine.fillAmount == 1)
            {
                GameManager.manager.level[GameManager.manager.currentLevel].stars = 3;
            }
            else if ((rewardLine.fillAmount >= 0.75f) && (GameManager.manager.level[GameManager.manager.currentLevel].stars < 3))
            {
                GameManager.manager.level[GameManager.manager.currentLevel].stars = 2;
            }
            else if ((rewardLine.fillAmount < 0.75f) && (GameManager.manager.level[GameManager.manager.currentLevel].stars < 2))
            {
                GameManager.manager.level[GameManager.manager.currentLevel].stars = 1;
            }
            if (rewardLine.fillAmount == 1) // These are used to calculate coins -- if already got 3 stars, but now got 2, pay 2.
                GameManager.manager.currentLevelStars = 3;
            else if (rewardLine.fillAmount >= 0.75f)
                GameManager.manager.currentLevelStars = 2;
            else
                GameManager.manager.currentLevelStars = 1;

            //update complete level details
            GameManager.manager.LevelComplete();
            
            //show the pass panel
            passPanel.SetActive(true);
            
            //USE TO prevent shooting
            bottomReached = true;
        }
        else // level failed
        {
            // Remove any errant balls
            StartCoroutine(ForceBallsToFinish());

            //slight delay
            for (int wait = 0; wait < 130; wait++)
            {
                yield return null;
            }
        
            failPanel.SetActive(true);
        }

        GameManager.manager.ballsActive = false;
    }

    public void BlockColour()
    {
        byte colour;

        GameObject[] block = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject b in block)
        {
            if (b != null) 
            {
                colour = (byte)(150 - (Mathf.RoundToInt(b.GetComponent<Block>().hitsRemaining / 50) * 20)); //50 points, 20 colour change

                b.GetComponent<Block>().colour = new Color32(0, (byte)(150 - b.GetComponent<Block>().hitsRemaining), 255, 255);
                //b.GetComponent<Block>().colour = new Color32(colour, colour, 255, 255);
                
                b.gameObject.GetComponent<SpriteRenderer>().color = b.GetComponent<Block>().colour;

                //ORANGE Warning when blocks are 2 level from the end
                if (b.transform.localPosition.y <= (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (levelGenerator.blockScaleAdjustedY * 3))
                {
                    b.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255,150,0,255);
                    b.gameObject.GetComponentInParent<Block>().colour = new Color32(255, 150, 0, 255);
                }
                //RED Warning when blocks are 1 level from the end
                if (b.transform.localPosition.y <= (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (levelGenerator.blockScaleAdjustedY * 1.5))
                {
                    b.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    b.gameObject.GetComponentInParent<Block>().colour = Color.red;
                }

            }
        }
    }

    public void ForceBallsToFinishWrapper()
    {
        StartCoroutine(ForceBallsToFinish());
    }

    IEnumerator ForceBallsToFinish()
    {
        ableToShoot = false;
        
        //stop balls moving.
        if (GameManager.manager.ballsActive == true)
        {
            GameManager.manager.ballsActive = false;

            if (GameManager.manager.actualNumberOfBlocks > 0)
            {
                StartCoroutine(GameManager.manager.Message("Shot Ended!", new Vector2(0, 0), 8, 1.5f, Color.white));
            }

            foreach (Ball b in ballControl.balls)
            {
                if (b.ball != null)
                {
                    AudioSource.PlayClipAtPoint(GameManager.manager.ballExplodeSound, b.ball.transform.localPosition);
                    //Rigidbody2D rb = b.ball.GetComponent<Rigidbody2D>();
                    //rb.velocity = new Vector2(0, 0);
                    Instantiate(ballExplosion, b.ball.transform.localPosition, Quaternion.identity);
                    Destroy(b.ball);

                }

                if (Random.Range(0, 2) == 1)
                    yield return null;// new WaitForSeconds(0.001f);
            }

            DestroySpecials();
            MoveBlocksDown();

            //Adjust colour for non 'bonus' levels
            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel !=0)
            {
                BlockColour();
            }
            else // this is only put in if bonus levels aren't using colour
            {
                BlockColour();
            }

            ResetPowerUps();

            //switch the bottompanel and shotpanel back
            StartCoroutine(SwapPanels());
        }
    }

    void DestroySpecials()
    {
        //Check that horizontal or vertical blocks been used, and destroy them if so.
        GameObject[] block = GameObject.FindGameObjectsWithTag("special"); // Not including super blocks...should I?
        foreach (GameObject b in block)
        {
            if ((b.GetComponent<HorizontalBlock>() != null) && (b.GetComponent<HorizontalBlock>().used == true))
            {
                Instantiate(ballExplosion, b.transform.localPosition, Quaternion.identity);
                Destroy(b);
            }

            if ((b.GetComponent<VerticalBlock>() != null) && (b.GetComponent<VerticalBlock>().used == true))
            {
                Instantiate(ballExplosion, b.transform.localPosition, Quaternion.identity);
                Destroy(b);
            }

        }
    }
    void BallsFinished()
    {
        if ((GameManager.manager.currentNumberOfBalls <= 0) && (GameManager.manager.ballsActive == true))
        {
            GameManager.manager.ballsActive = false;
            ableToShoot = false;

            MoveBlocksDown();

            //COLOUR FOR SPECIAL LEVELS TO NOT BE DONE
            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel != 0)
            {
                BlockColour();
            }
            else
            {
                BlockColour(); //this wasn't here before when doing the colours icon type levels.
            }

            ResetPowerUps();

            DestroySpecials();
            //switch the bottompanel and shotpanel back
            StartCoroutine(SwapPanels());
        }
    }

    void LaunchBalls()
    {
        if ((ableToShoot == true) && (GameManager.manager.ballsActive == false) && (bottomReached==false))
        {
            //used to show HS is new HS 
            hs = false;

            GameManager.manager.ballsActive = true;
            ableToShoot = false;

            ballControl.InitializeBalls(startPos);
            StartCoroutine(ballControl.MoveBalls(startPos, endPos)); // Starts the balls moving, 

            //set current shot score to 0
            GameManager.manager.level[GameManager.manager.currentLevel].shotPoints = 0;
            //increase shot count
            shotsTaken++;

            StartCoroutine(SwapPanels());
        }
    }
    void UpdateRewardLine()
    {
        float currentShotPercentage = 0;
        float highBreakEven = 55;
        float mediumBreakEven = 38;
        float lowBreakEven = 25;

        levelPercentage = 0;

        if (GameManager.manager.ballsActive == true)
        {
            if ((GameManager.manager.currentNumberOfBalls > 0) && (GameManager.manager.ballsActive == true))
            {
                float p = 0.85f;

                //get % of shot to total level points
                currentShotPercentage = ((float)(GameManager.manager.level[GameManager.manager.currentLevel].shotPoints + 1) / (float)GameManager.manager.totalLevelPoints) * 100;
           
                if (currentShotPercentage > 0)
                {
                    if(GameManager.manager.currentLevel < 50)
                    {
                        levelPercentage = ((currentShotPercentage / 100) * (currentShotPercentage / highBreakEven));
                    }
                    else if(GameManager.manager.currentLevel < 100)
                    {
                        levelPercentage = ((currentShotPercentage / 100) * (currentShotPercentage / mediumBreakEven));
                    }
                    else
                    {
                        levelPercentage = ((currentShotPercentage / 100) * (currentShotPercentage / lowBreakEven));
                    }
                }
                        
                //update reward line
                rewardLine.fillAmount = oldFillAmount + levelPercentage;

                //update smiley faces
                if (rewardLine.fillAmount >= 1)
                {
                    laughingFace.color = new Color32(255, 255, 255, 255);
                    happyFace.color = new Color32(255, 255, 255, 255);
                    neutralFace.color = new Color32(255, 255, 255, 255);
                }
                else if (rewardLine.fillAmount >= 0.75)
                {
                    happyFace.color = new Color32(255, 255, 255, 255);
                    neutralFace.color = new Color32(255, 255, 255, 255);
                }
                else
                {   
                    neutralFace.color = new Color32(255, 255, 255, 255);
                }
            }

        }
        else
            oldFillAmount = rewardLine.fillAmount; //store for next shot
    }
    void UpdateLevelText()
    {
        //update points shown
        shotScoreText.text = ("Score: " + GameManager.manager.level[GameManager.manager.currentLevel].shotPoints);
        //update highest score if necessary
        if (GameManager.manager.level[GameManager.manager.currentLevel].shotPoints > GameManager.manager.level[GameManager.manager.currentLevel].highestPoints)
        {
            if((hs==false) && (GameManager.manager.firstRun == false))
            {
                StartCoroutine(GameManager.manager.Message("New High Score!", new Vector3(0, 0, 0), 8, 1.5f, Color.white));
                hs = true;
            }
            

            GameManager.manager.level[GameManager.manager.currentLevel].highestPoints = GameManager.manager.level[GameManager.manager.currentLevel].shotPoints;
            if (GameManager.manager.firstRun != true) //Avoid highscore on first run when highest score is 0
            {
                GameManager.manager.newHighScore = true;
            }
        }
        highScoreText.text = ("High: " + GameManager.manager.level[GameManager.manager.currentLevel].highestPoints);

        //update shots taken
        shotsTakenText.text = ("Shots: " + shotsTaken);
        //Show the number of balls to play this shot
        numberOfBallsText.text = GameManager.manager.maxNumberOfBalls + " Balls This Shot";
        //Show the number of balls left in play
        if(GameManager.manager.currentNumberOfBalls > 1)
            numberOfBallsInPlayText.text = GameManager.manager.currentNumberOfBalls + " Balls In Play...";
        else
            numberOfBallsInPlayText.text = GameManager.manager.currentNumberOfBalls + " Ball In Play...";
        //Show playerCoins
        playerCoinsText.text = GameManager.manager.playerCoins.ToString();
        
        //Show current number of powerups
        //Double Balls
        if (GameManager.manager.numberOfBalls2x > 0)
        {
            balls2xCoins.SetActive(false);
            balls2xText.text = GameManager.manager.numberOfBalls2x.ToString() + " x";
        }
        else
        {
            balls2xCoins.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.manager.balls2xCost.ToString();
            balls2xCoins.SetActive(true);
            balls2xText.text = "";
        }
        //Block Reduction
        if (GameManager.manager.numberOfBockReductions > 0)
        {
            blockReductionCoins.SetActive(false);
            blockReductionText.text = GameManager.manager.numberOfBockReductions.ToString() + " x";
        }
        else
        {
            blockReductionCoins.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.manager.blockReductionCost.ToString();
            blockReductionCoins.SetActive(true);
            blockReductionText.text = "";
        }
        //Floor Blocks
        if (GameManager.manager.numberOfFloorBlocks > 0)
        {
            floorBlockCoins.SetActive(false);
            floorBlockText.text = GameManager.manager.numberOfFloorBlocks.ToString() + " x";
        }
        else
        {
            floorBlockCoins.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.manager.floorBlockCost.ToString();
            floorBlockCoins.SetActive(true);
            floorBlockText.text = "";
        }
        //InvincibleBalls
        if (GameManager.manager.numberOfInvincibleBalls > 0)
        {
            invincibleBallsCoins.SetActive(false);
            invincibleBallsText.text = GameManager.manager.numberOfInvincibleBalls.ToString() + " x";
        }
        else
        {
            invincibleBallsCoins.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.manager.invincibleBallsCost.ToString();
            invincibleBallsCoins.SetActive(true);
            invincibleBallsText.text = "";
        }
        
    
    }
    public void MoveBlocksDown() // called from BALLS FINSISHED and check for end of level due to blocks hitting bottom and forced balls finished
    {
        if (GameManager.manager.keepWaiting == false)
        {
            GameObject[] block = GameObject.FindGameObjectsWithTag("block");
            GameObject[] solid = GameObject.FindGameObjectsWithTag("solidBlock");
            GameObject[] bombs = GameObject.FindGameObjectsWithTag("bomb");
            GameObject[] special = GameObject.FindGameObjectsWithTag("special");
            List<GameObject> items = new List<GameObject>(block);
            items.AddRange(new List<GameObject>(solid)); // get all the blocks, normal and super
            items.AddRange(new List<GameObject>(bombs)); // and bombs
            items.AddRange(new List<GameObject>(special));// and specials move the blocks down 
            foreach (GameObject b in items)
            {
                //Move blocks down
                b.transform.localPosition = new Vector2(b.transform.localPosition.x, b.transform.localPosition.y - levelGenerator.blockScaleAdjustedY);
                /*
                //Warning when blocks are 1 level from the end
                if (b.transform.localPosition.y <= (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (levelGenerator.blockScaleAdjustedY * 1.5))
                {
                    //b.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    b.gameObject.GetComponentInParent<Block>().colour = Color.red;

                }
                */
                //if blocks have reached the bottom of the screen, show fail screen
                if (b.transform.localPosition.y <= (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (levelGenerator.blockScaleAdjustedY / 2))
                {
                    //GameManager.manager.ballsActive = true; //used to prevent DRAGTOSHOOT allowing to play in background. Change it to false in CONTINUE method.
                    if ((b.tag == "solidBlock")||(b.tag=="special")||(b.tag=="bomb"))
                    {
                        Instantiate(ballExplosion, b.transform.localPosition, Quaternion.identity);
                        Destroy(b);
                    }
                    else
                    {

                        //levelFailed = true;
                        //failPanel.SetActive(true);
                        StartCoroutine(ShowFailPanelWithDelay(0.5f));

                        //used to prevent LAUNCHBALLS from working
                        bottomReached = true;
                    }
                }
            }
        }
    }

    IEnumerator ShowFailPanelWithDelay(float d)
    {
        //play fail sound
        AudioSource.PlayClipAtPoint(GameManager.manager.levelFailSound, gameObject.transform.localPosition);
        yield return new WaitForSeconds(d);
        failPanel.SetActive(true);
    }

    public void Continue() // called from the continue button on a failed level
    { 
        int upAmount = 1;
        int levelsToDestroy = 2;

        //Hide the fail panel
        failPanel.SetActive(false);

        //Move the blocks back up
        GameObject[] block = GameObject.FindGameObjectsWithTag("block");
        GameObject[] solid = GameObject.FindGameObjectsWithTag("solidBlock");
        GameObject[] bomb = GameObject.FindGameObjectsWithTag("bomb");
        GameObject[] special = GameObject.FindGameObjectsWithTag("special");
        List<GameObject> items = new List<GameObject>(block);
        items.AddRange(new List<GameObject>(solid)); // get all blocks, including super;
        items.AddRange(new List<GameObject>(bomb));
        items.AddRange(new List<GameObject>(special));

        foreach (GameObject b in items)
        {
            //destroy bottom 2 layers
            if ((b.transform.localPosition.y <= (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + ((levelGenerator.blockScaleAdjustedY * levelsToDestroy))))
            {
                Instantiate(ballExplosion, b.transform.localPosition, Quaternion.identity);
                if(b.tag=="block")
                {
                    GameManager.manager.actualNumberOfBlocks--;
                }
                
                Destroy(b);
            }
            else // move blocks up if they haven't been exploded
            {
                for (int up = 0; up < upAmount; up++)
                {
                    //Move blocks up
                    b.transform.localPosition = new Vector2(b.transform.localPosition.x, b.transform.localPosition.y + levelGenerator.blockScaleAdjustedY);
                    // some kind of delay here.
                }
            }
        }

        //Re-adjust block colours
        BlockColour();

        //Remove payment from player coins
        GameManager.manager.playerCoins -= GameManager.manager.continueCost;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        //Set back to false to allow DRAGTOSHOOT to work
        bottomReached = false;
        

    }
 
    void ResetPowerUps()
    {
        //reset double balls
        if (balls2x.doubleBalls == true)
        {
            balls2x.doubleBalls = false;
            GameManager.manager.maxNumberOfBalls /= 2;
        }
        //reset block reduction
        if (blockReduction.blockReductionActive == true)
        {
            blockReduction.blockReductionActive = false;
        }
        //reset floor blocks
        if(floorBlock.floorBlocksActive == true)
        {
            floorBlock.floorBlocksActive = false;
            //Destroy floorblocks
            GameObject[] block = GameObject.FindGameObjectsWithTag("floorBlock");
            foreach (GameObject b in block)
            {
                Instantiate(ballExplosion, b.transform.localPosition, Quaternion.identity);
                Destroy(b);
            }
           

        }
        //Reset Power Balls
        if(invincibleBalls.invincibleBallsActive == true)
        {
            invincibleBalls.invincibleBallsActive = false;

            //Set all blocks back to not being triggers
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
            foreach (GameObject b in blocks)
            {
                b.GetComponentInChildren<Collider2D>().isTrigger = false;
            }
        }
        
    }
}

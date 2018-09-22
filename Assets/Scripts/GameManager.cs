using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour {

    bool debug = false;

    public static GameManager manager;

    [HideInInspector]
    public float camY, camX;

    [HideInInspector]
    public float freeTopArea;// = 0.01f; // 20% - use as size of top and bottom borders -- was 0.15
    [HideInInspector]
    public float freeBottomArea;// = 0.01f;

    //[HideInInspector]
    //public int highestNumberOfHits;
    //[HideInInspector]
    //public int lowestNumberOfHits;

    [HideInInspector]
    public int maxNumberOfBalls;
    public int baseNumberOfBalls;

    [Header("Ball Details")]
    public GameObject ballNormal;

    [HideInInspector]
    public int currentNumberOfBalls;
    
    [Header("Level Prefabs")]
    public GameObject levelReady;
    public GameObject levelLocked;
    

    [HideInInspector]
    public struct Level
    {
        //public int levelNumber;
        public int stars;
        public int highestPoints;
        public int shotPoints;
        public int lowestShotsTaken;
        public int shotsTaken;  
    }

    [HideInInspector]
    public int highestShotEver;

    [HideInInspector]
    public Level[] level;

    [HideInInspector]
    public int levelCount, currentLevel, highestLevelPlayed;
    [HideInInspector]
    public bool ballsActive;
    [HideInInspector]
    public int actualNumberOfBlocks;
    public int totalLevelPoints;
    public int playerCoins;
    public int continueCost;
    public double warningArea;
    public int starCoins1, starCoins2, starCoins3;
    public int newHighScoreCoins;
    public int numberOfBalls2x;
    public int numberOfBockReductions;
    public int numberOfFloorBlocks;
    public int numberOfInvincibleBalls;
    public int balls2xCost;
    public int floorBlockCost;
    public int invincibleBallsCost;
    public int blockReductionCost;
    public int currentLevelStars;
    
    //[HideInInspector]
    //public GameObject[] blocks;
    [HideInInspector]
    public PlayLevel playlevel;

    const string stars = "stars";
    const string highestPoints = "highestPoints";
    const string lowestShotsTaken = "lowestShotsTaken";
    const string highestLevel = "highestLevel";
    //powerup strings
    public string balls2x = "balls2x";
    public string blockReduction = "blockReduction";
    public string floorBlock = "floorBlock";
    public string invincibleBalls = "invincibleBalls";

    public int bonusLevel = 10;

    void Awake()
    {
        // Ensure this is the only manager, and make it dontdestroyonload.
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //Screen units
        camY = Camera.main.orthographicSize * 2; //Full sprHeight
        camX = (camY * Screen.width / Screen.height); // covers different aspect ratios
        
        freeTopArea = 0.15f; // 15% - use as size of top and bottom borders -- was 0.15
        freeBottomArea = 0.135f;

        baseNumberOfBalls = 50;
        maxNumberOfBalls = 50;
    

        //stars rewards
        starCoins1 = 15;
        starCoins2 = 25;
        starCoins3 = 35;

        newHighScoreCoins = 10;

        //Powerups
        balls2xCost = 200;
        floorBlockCost = 400;
        invincibleBallsCost = 500;
        blockReductionCost = 300;
        continueCost = 100;

        playerCoins = PlayerPrefs.GetInt("playerCoins");

        SetupLevels();

        //Just for development - remove later
        ClearLevels();

        InitializeLevels();
    }

    void SetupLevels()
    {
        //Get the number of levels designed
        //levelCount = Directory.GetFiles(Application.dataPath + "/resources/", "*.png", SearchOption.AllDirectories).Length;
        //Debug.Log("Levels = " + levelCount);
        //levelCount = 12;

        Object[] maps = Resources.LoadAll("Levels", typeof(Texture2D));
        levelCount = maps.Length; 
        Resources.UnloadUnusedAssets();

        //Set up the level array to the size of the levels created
        level = new Level[levelCount + 1];
    }

    void ClearLevels()
    {
        for(int x = 0; x<=levelCount+1; x++)
        {
            PlayerPrefs.SetInt("playerCoins", 0);
            PlayerPrefs.SetInt("level" + x + stars, 0);
            PlayerPrefs.SetInt("level" + x + highestPoints, 0);
            PlayerPrefs.SetInt("level" + x + lowestShotsTaken, 0);

        }

        //Powerups
        PlayerPrefs.SetInt(balls2x, 1);
        PlayerPrefs.SetInt(blockReduction, 1);
        PlayerPrefs.SetInt(invincibleBalls, 1);
        PlayerPrefs.SetInt(floorBlock, 1);

        if (debug)
        {
            PlayerPrefs.SetInt(highestLevel, 1000);
            highestLevelPlayed = 1000;
            playerCoins = 1000;
            PlayerPrefs.SetInt("playerCoins", playerCoins);
            //powerups
            PlayerPrefs.SetInt(balls2x, 1);
            PlayerPrefs.SetInt(blockReduction, 1);
            PlayerPrefs.SetInt(invincibleBalls, 1);
            PlayerPrefs.SetInt(floorBlock, 1);
        }
        else
        { 
            PlayerPrefs.SetInt(highestLevel, 1);

        }
    }

    public void InitializeLevels()
    {
        //Get the all level stars and high score
        for(int x = 1; x <= levelCount; x++)
        {
            level[x].stars = PlayerPrefs.GetInt("level" + x + stars); //return 0 if not set
            level[x].highestPoints = PlayerPrefs.GetInt("level" + x + highestPoints); //return 0 if not set
            level[x].lowestShotsTaken = PlayerPrefs.GetInt("level" + x + lowestShotsTaken);//return 0 if not set
        }

        // First time played, give the player powerups
        if (PlayerPrefs.GetInt("firstTimePlayed") == 0)
        {
            PlayerPrefs.SetInt(balls2x, 1);
            PlayerPrefs.SetInt(blockReduction, 1);
            PlayerPrefs.SetInt(floorBlock, 1);
            PlayerPrefs.SetInt(invincibleBalls, 1);

            PlayerPrefs.SetInt("firstTimePlayed", 1); // set so they don't get powerups each time they play
        }

        //Get powerups
        numberOfBalls2x = PlayerPrefs.GetInt(balls2x);
        numberOfBockReductions = PlayerPrefs.GetInt(blockReduction);
        numberOfFloorBlocks = PlayerPrefs.GetInt(floorBlock);
        numberOfInvincibleBalls = PlayerPrefs.GetInt(invincibleBalls);

        //Get player coins
        playerCoins = PlayerPrefs.GetInt("playerCoins"); // returns 0 if nothing

        // Get the highest level
        highestLevelPlayed = PlayerPrefs.GetInt(highestLevel); //return 0 if not set
        //If never played, ensure first level is available in populate levels
        if (highestLevelPlayed == 0)
            PlayerPrefs.SetInt(highestLevel, 1);

        //***********DEBUG
        if (debug == true)
        {
            PlayerPrefs.SetInt(highestLevel, 1000);
            highestLevelPlayed = 1000;
        }
    }

    public void LevelComplete()
    {
        //Allocate coins depending on stars received 
        if(currentLevel % bonusLevel == 0)
        {
            switch (currentLevelStars)//level[currentLevel].stars)
            {
                case 1:
                    playerCoins += starCoins1*2;
                    break;
                case 2:
                    playerCoins += starCoins2*2;
                    break;
                case 3:
                    playerCoins += starCoins3*2;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (currentLevelStars)//level[currentLevel].stars)
            {
                case 1:
                    playerCoins += starCoins1;
                    break;
                case 2:
                    playerCoins += starCoins2;
                    break;
                case 3:
                    playerCoins += starCoins3;
                    break;
                default:
                    break;
            }
        }
        PlayerPrefs.SetInt("playerCoins", playerCoins);

        //Set the stars for completed level
        //PlayerPrefs.SetInt("level" + currentLevel + stars, level[currentLevel].stars);

        //Check if at highest level, if so Allow the next level to be played
        if((currentLevel==PlayerPrefs.GetInt(highestLevel)) && (highestLevelPlayed < levelCount+1))
        {
            PlayerPrefs.SetInt(highestLevel, currentLevel + 1);
        }

        //Save the highest shot score -- this will have been updated in the playlevel
        PlayerPrefs.SetInt("level" + currentLevel + highestPoints,level[currentLevel].highestPoints);

        //Save new lowest number of shots taken if needed
        if((level[currentLevel].shotsTaken < PlayerPrefs.GetInt("level"+currentLevel+lowestShotsTaken)) || (PlayerPrefs.GetInt("level"+currentLevel+lowestShotsTaken)==0))
        {
            PlayerPrefs.SetInt("level" + currentLevel + lowestShotsTaken, level[currentLevel].lowestShotsTaken);
        }
    }

    public IEnumerator Message(string txt)
    {
        GameObject dbiTextObject = new GameObject();
        TextMeshProUGUI dbiText;
        float deathTime = 1.5f;
        float elapsedTime = 0;
        Vector2 startingScale = new Vector2(8, 8);
        Vector2 endingScale = new Vector2(0, 0);

        dbiTextObject.transform.parent = GameObject.Find("Canvas").transform;
        dbiText = dbiTextObject.AddComponent<TextMeshProUGUI>();
        dbiText.alignment = TextAlignmentOptions.Center;
        dbiText.autoSizeTextContainer = true;
        dbiText.text = txt;

        dbiText.transform.localPosition = new Vector2(0, 0);

        while (elapsedTime < deathTime)
        {
            dbiText.transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / deathTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Destroy(dbiTextObject);
    }


}

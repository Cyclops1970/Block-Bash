﻿using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;


public class LevelGenerator : MonoBehaviour {
    //[HideInInspector]
    public static LevelGenerator levelGenerator { get; set; }

    public ColourToPrefab[] colourMappings;
    public LevelMaker level; //scriptable object -- array of colourmappings.

    [HideInInspector]
    public Texture2D currentLevel;

    GameObject[] blocks;
    public GameObject[,] block;

    public GameObject plainBlock;

    [HideInInspector]
    public int maxNumberOfBlocks;

    //Text hitsRemainingText;
    TextMeshProUGUI hitsRemainingText;
        
    [HideInInspector]
    public float xStart, yStart;

    float blockPercentageSize;
    float blockPercentageSizeX, blockPercentageSizeY;
    [HideInInspector]
    public float blockScaleAdjustedX;
    [HideInInspector]
    public float blockScaleAdjustedY;

    GameObject blockContainer;

    private void Awake()
    {
        levelGenerator = this;
    }

    public void GenerateLevel()
    {
        //load the current level map for the png file
        currentLevel = Resources.Load<Texture2D>("Levels/"+GameManager.manager.currentLevel.ToString());
        if (currentLevel == null)
        {
            Debug.Log("Level NOT loaded!");
        }
        
        InitializeLevel();
    }

    void InitializeLevel()
    {
        blockContainer = new GameObject();
        blockContainer.name = "blockContainer";

        //reset actual number of blocks in play;
        GameManager.manager.actualNumberOfBlocks = 0;
        //reset total level points
        GameManager.manager.totalLevelPoints = 0;

        blockPercentageSizeX = (1.0f / (float)currentLevel.width); // % of screenwidth taken by 1 block 
        blockPercentageSizeY = (1.0f / ((float)currentLevel.height + 1));//Add 1 to ensure there is alway a gap to start with
        
        //blocks = new GameObject[currentLevel.height * currentLevel.width];
        block = new GameObject[currentLevel.width, currentLevel.height+1];

        for (int y = 0; y < currentLevel.height; y++) 
        {
            for(int x = 0; x < currentLevel.width; x++)
            {
                if(GameManager.manager.currentLevel % GameManager.manager.bonusLevel != 0)
                {
                    //GenerateBlock(x, y);
                    GenerateNewBlock(x, y);
                }
                else
                {
                    //GenerateBonusBlock(x, y);
                    GenerateNewBlock(x, y);
                }
            }
        }
        StartCoroutine(ScaleBlocks());
        AudioSource.PlayClipAtPoint(GameManager.manager.levelStartSound, gameObject.transform.localPosition);
    }

    void GenerateNewBlock(int x, int y)
    {
        float cX = Camera.main.orthographicSize * 2;
        float cY = (cX * Screen.height / Screen.width);

        Color pixelColour = currentLevel.GetPixel(x, currentLevel.height - 1 - y); //bottom up.

        blockScaleAdjustedX = ScaleGameObjectToScreenPercentageX(plainBlock, blockPercentageSizeX);
        blockScaleAdjustedY = ScaleGameObjectToScreenPercentageY(plainBlock, blockPercentageSizeY); //blocknormal

        // Get starting pos X (left hand side of screen), y (top of screen)
        xStart = (-GameManager.manager.camX / 2 + blockScaleAdjustedX / 2);
        yStart = (GameManager.manager.camY / 2 - blockScaleAdjustedY / 2) - (GameManager.manager.camY * GameManager.manager.freeTopArea);
        
        //go through colour mappings and instantiate according to colour.
        foreach (ColourToPrefab currentBlock in level.ctp)
        {
            if (currentBlock.colour.Equals(pixelColour))
            {
                block[x, y] = Instantiate(currentBlock.blockType);
                block[x, y].gameObject.transform.SetParent(blockContainer.transform); // make it neater in the heirarchy

                //Scale block correctly
                block[x, y].GetComponent<Block>().transform.localScale = new Vector2(blockScaleAdjustedX, blockScaleAdjustedY);
                //Place block
                block[x, y].gameObject.transform.localPosition = new Vector2(xStart + (blockScaleAdjustedX * x), yStart - (blockScaleAdjustedY * y));

                //white is solid, red is bomb, light red is vertical, medium red is horizontal
                if ((pixelColour != Color.white) && (pixelColour != Color.red) && (pixelColour != new Color(1, 0, 0, 0.2f)) && (pixelColour != new Color(1, 0, 0, 0.4f)))
                {
                    //Initial Colour
                    if (y == currentLevel.height - 1)
                    {
                        block[x, y].GetComponent<Block>().colour = Color.red;
                    }
                    else
                    {
                        block[x, y].GetComponent<Block>().colour = new Color32(0, (byte)(200 - currentBlock.hitsToKill / 2), 255, 255);
                    }
                    block[x, y].gameObject.GetComponent<SpriteRenderer>().color = block[x, y].GetComponent<Block>().colour;

                    //set and display number of hits 
                    block[x, y].GetComponent<Block>().hitsRemaining = currentBlock.hitsToKill;
                    hitsRemainingText = block[x, y].gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(); // get the textmeshpro element of the letterText
                    hitsRemainingText.text = block[x, y].GetComponent<Block>().hitsRemaining.ToString();

                    //update total level points 
                    GameManager.manager.totalLevelPoints += currentBlock.hitsToKill;
                    //keep count of blocks used, so we know end of level when all blocks removed
                    GameManager.manager.actualNumberOfBlocks++;
                }
               // block[x, y].gameObject.transform.rotation = Random.rotation;  //////use for some kind of easter egg??!!!!!!!!!!!!lakjdhgf;auids;uiashd;ia;dsgfinasd;gikna;g
            }
        }

    }

    IEnumerator ScaleBlocks()
    {
        float elapsedTime = 0;
        float scaleTime = 1;
        Vector2 startingScale = new Vector2(0, 0);
        Vector2 endingScale = new Vector2(blockScaleAdjustedX, blockScaleAdjustedY);


        while (elapsedTime < scaleTime)
        {
            for (int y = 0; y < currentLevel.height; y++)
            {
                for (int x = 0; x < currentLevel.width; x++)
                {
                    if(block[x,y]!=null)
                    {
                        block[x, y].gameObject.transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / scaleTime));
                    }
                }
            }
            elapsedTime += Time.deltaTime;
            //yield return new WaitForSecondsRealtime(0.01f);
            yield return null;
        }

        //scale all blocks to full size as lerp sometimes fails
        for (int y = 0; y < currentLevel.height; y++)
        {
            for (int x = 0; x < currentLevel.width; x++)
            {
                if(block[x,y]!=null)
                {
                    block[x, y].gameObject.transform.localScale = endingScale;
                }
            }
        }
    }
    /*

    void GenerateBlock(int x, int y)
    {
        Color pixelColour = currentLevel.GetPixel(x, currentLevel.height - 1 - y);

        // get the correct scale to fit the number of blocks across and down the play area
        //blockScaleAdjustedX = ScaleGameObjectToScreenPercentageX(colourMappings[0].blockType, blockPercentageSizeX);
        blockScaleAdjustedX = ScaleGameObjectToScreenPercentageX(level.ctp[0].blockType, blockPercentageSizeX);
        blockScaleAdjustedY = ScaleGameObjectToScreenPercentageY(level.ctp[0].blockType, blockPercentageSizeY); //blocknormal
        //blockScaleAdjustedY = ScaleGameObjectToScreenPercentageY(colourMappings[0].blockType, blockPercentageSizeY); //blocknormal

        // Get starting pos X (left hand side of screen), y (top of screen)
        xStart = (-GameManager.manager.camX / 2 + blockScaleAdjustedX / 2);
        yStart = (GameManager.manager.camY / 2 - blockScaleAdjustedY / 2) - (GameManager.manager.camY * GameManager.manager.freeTopArea);

        //go through colour mappings and instantiate according to colour.
        //foreach(ColourToPrefab currentBlock in colourMappings)
        //{
        foreach (ColourToPrefab currentBlock in level.ctp)
        {
            if (currentBlock.colour.Equals(pixelColour))
            {
                blocks[y * x] = Instantiate(currentBlock.blockType);
                blocks[y * x].transform.SetParent(blockContainer.transform); // make it neater in the heirarchy

                //Scale block correctly
                blocks[y * x].GetComponent<Block>().transform.localScale = new Vector2(blockScaleAdjustedX, blockScaleAdjustedY);
                //Place block
                blocks[y * x].gameObject.transform.localPosition = new Vector2(xStart + (blockScaleAdjustedX * x), yStart - (blockScaleAdjustedY * y));
                
                //white is solid, red is bomb, light red is vertical, medium red is horizontal
                if ((pixelColour != Color.white) && (pixelColour != Color.red)&&(pixelColour != new Color(1,0,0,0.2f))&&(pixelColour != new Color(1,0,0,0.4f)))
                {
                    //Initial Colour
                    if (y == currentLevel.height - 1)
                    {
                        blocks[y * x].GetComponent<Block>().colour = Color.red;
                    }
                    else
                    {
                        blocks[y * x].GetComponent<Block>().colour = new Color32(0, (byte)(200 - currentBlock.hitsToKill / 2), 255, 255);
                    }
                    blocks[y * x].gameObject.GetComponent<SpriteRenderer>().color = blocks[y * x].GetComponent<Block>().colour;

                    //set and display number of hits 
                    blocks[y * x].GetComponent<Block>().hitsRemaining = currentBlock.hitsToKill;
                    hitsRemainingText = blocks[y * x].gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(); // get the textmeshpro element of the letterText
                    hitsRemainingText.text = blocks[y * x].GetComponent<Block>().hitsRemaining.ToString();

                    //update total level points 
                    GameManager.manager.totalLevelPoints += currentBlock.hitsToKill;
                    //keep count of blocks used, so we know end of level when all blocks removed
                    GameManager.manager.actualNumberOfBlocks++;
                }
            }

        }

    }

*/
    void GenerateBonusBlock(int x, int y)
    {
        Color pixelColour = currentLevel.GetPixel(x, currentLevel.height - 1 - y);

        // get the correct scale to fit the number of blocks across and down the play area
        blockScaleAdjustedX = ScaleGameObjectToScreenPercentageX(level.ctp[0].blockType, blockPercentageSizeX);
        blockScaleAdjustedY = ScaleGameObjectToScreenPercentageY(level.ctp[0].blockType, blockPercentageSizeY); //blocknormal
        
        // Get starting pos X (left hand side of screen), y (top of screen)
        xStart = (-GameManager.manager.camX / 2 + blockScaleAdjustedX / 2);
        yStart = (GameManager.manager.camY / 2 - blockScaleAdjustedY / 2) - (GameManager.manager.camY * GameManager.manager.freeTopArea);

        //go through colour mappings and instantiate according to colour.
        //foreach(ColourToPrefab currentBlock in colourMappings)
        //{

        if (pixelColour.a != 0)
        {
            blocks[y * x] = Instantiate(plainBlock);

            //keep count of blocks used, so we know end of level when all blocks removed
            GameManager.manager.actualNumberOfBlocks++;

            //Scale block correctly
            blocks[y * x].GetComponent<Block>().transform.localScale = new Vector2(blockScaleAdjustedX, blockScaleAdjustedY);
            //Place block
            blocks[y * x].gameObject.transform.localPosition = new Vector2(xStart + (blockScaleAdjustedX * x), yStart - (blockScaleAdjustedY * y));
            //use with solid blocks?
            //ocks[y * x].GetComponent<Block>().active = true;
            //Initial Colour
            if (y == currentLevel.height - 1)
            {
                blocks[y * x].GetComponent<Block>().colour = Color.red;
            }
            else
            {
                blocks[y * x].GetComponent<Block>().colour = pixelColour;
            }
            blocks[y * x].gameObject.GetComponent<SpriteRenderer>().color = blocks[y * x].GetComponent<Block>().colour;

            //set and display number of hits 
            blocks[y * x].GetComponent<Block>().hitsRemaining = 1;

            //update total level points 
            GameManager.manager.totalLevelPoints += 1;
        }
        

    }
    public float ScaleGameObjectToScreenPercentageX(GameObject g, float p)
    {
        float scale = 0;
        float newSize;
        float gameObjectWidth = g.GetComponent<SpriteRenderer>().sprite.bounds.size.x;

        newSize = GameManager.manager.camX * p;
        scale = (g.transform.localScale.x / gameObjectWidth) * newSize;
        
        return (scale);
    }
    public float ScaleGameObjectToScreenPercentageY(GameObject g, float p)
    {
        float scale = 0;
        float newSize;
        float gameObjectHeight = g.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        float playArea = GameManager.manager.camY - (GameManager.manager.camY*GameManager.manager.freeTopArea) - (GameManager.manager.camY*GameManager.manager.freeBottomArea);
        
        newSize = playArea * p;
        scale = (g.transform.localScale.y / gameObjectHeight) * newSize;
        
        return (scale);
    }
}

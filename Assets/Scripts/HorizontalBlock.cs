using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HorizontalBlock : MonoBehaviour
{
    LevelGenerator levelGenerator;

    [HideInInspector]
    public HorizontalBlock horizontalBlock;
    [HideInInspector]
    public bool used;
    [HideInInspector]
    public TextMeshProUGUI hitsRemainingText;
    public GameObject explode;

    //line stuff
    private LineRenderer lRend;
    private Vector3 transformPointA;
    private Vector3 transformPointB;
    public Material lightningMaterial;
    private Vector3[] points;
    private readonly int pointsCount = 5;
    private int xPos, yPos;

    private void Start()
    {
        //Never been used
        used = false;

        //used for lines
        gameObject.AddComponent<LineRenderer>();
        lRend = GetComponent<LineRenderer>();
        points = new Vector3[pointsCount];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // find the array element
        for (int y = 0; y < LevelGenerator.levelGenerator.currentLevel.height; y++)
        {
            for (int x = 0; x < LevelGenerator.levelGenerator.currentLevel.width; x++)
            {
                if (LevelGenerator.levelGenerator.block[x, y] != null)
                {
                    if ((LevelGenerator.levelGenerator.block[x, y].transform.localPosition == gameObject.transform.localPosition))
                    {
                        xPos = x;
                        yPos = y;
                    }
                }
            }
        }

        //sound
        AudioSource.PlayClipAtPoint(GameManager.manager.electrocutionSound, gameObject.transform.localPosition, 100);

        for (int x = 0; x < LevelGenerator.levelGenerator.currentLevel.width; x++)
        {
            if (LevelGenerator.levelGenerator.block[x, yPos] != null)
            {
                if (LevelGenerator.levelGenerator.block[x, yPos].tag != "special")
                {
                    StartCoroutine(FlashBlock(LevelGenerator.levelGenerator.block[x, yPos].gameObject));
                }
                LevelGenerator.levelGenerator.block[x, yPos].GetComponent<Block>().hitsRemaining--;
                GameManager.manager.level[GameManager.manager.currentLevel].shotPoints++;

                //Adjust hitsRemainingText
                if (LevelGenerator.levelGenerator.block[x, yPos].GetComponentInParent<Block>().hitsRemaining > 0)
                {
                    hitsRemainingText = LevelGenerator.levelGenerator.block[x, yPos].GetComponentInChildren<TextMeshProUGUI>();
                    hitsRemainingText.text = LevelGenerator.levelGenerator.block[x, yPos].GetComponentInParent<Block>().hitsRemaining.ToString();
                }
                else
                {
                    if (LevelGenerator.levelGenerator.block[x, yPos].tag == "block")
                    {
                        LevelGenerator.levelGenerator.block[x, yPos].GetComponent<Collider2D>().enabled = false;
                        hitsRemainingText = LevelGenerator.levelGenerator.block[x, yPos].GetComponentInChildren<TextMeshProUGUI>();
                        hitsRemainingText.text = "0";
                        StartCoroutine(BlockDeath(LevelGenerator.levelGenerator.block[x, yPos].gameObject, x));
                    }
                }

                if(LevelGenerator.levelGenerator.block[x,yPos]!=null)
                    BlockColour(LevelGenerator.levelGenerator.block[x, yPos].gameObject);

            }
        }


        /*
        GameObject[] block = GameObject.FindGameObjectsWithTag("block"); // Not including super blocks...should I?
        foreach (GameObject b in block)
        {
            //checked if same level
            if(b.transform.localPosition.y == transform.localPosition.y)
            {
                StartCoroutine(FlashBlock(b));
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.electrocutionSound, gameObject.transform.localPosition, 100);

                //reduce hits left and increase score
                b.GetComponentInParent<Block>().hitsRemaining--;
                GameManager.manager.level[GameManager.manager.currentLevel].shotPoints++;

                //Adjust hitsRemainingText
                if (b.GetComponentInParent<Block>().hitsRemaining > 0)
                {
                    hitsRemainingText = b.GetComponentInChildren<TextMeshProUGUI>();
                    hitsRemainingText.text = b.GetComponentInParent<Block>().hitsRemaining.ToString();
                }
                else
                {
                    b.GetComponent<Collider2D>().enabled = false;
                    hitsRemainingText = b.GetComponentInChildren<TextMeshProUGUI>();
                    hitsRemainingText.text = "0";
                    StartCoroutine(BlockDeath(b));
                }
            }
        }
        */

        Line();
    }

    void BlockColour(GameObject gameObject)
    {
        byte colour;
        Color32 oldColour;
        Color32 orange = new Color32(255, 150, 0, 255);
        Color32 red = Color.red;

        if (gameObject != null)
        {
            // Get current block colour
            oldColour = gameObject.GetComponent<Block>().colour;

            //Don't update if orange or red
            if ((!oldColour.Equals(orange)) && (!oldColour.Equals(red)) && (gameObject.tag!="special") && (gameObject.tag!="solidBlock") && (gameObject.tag!="bomb"))
            {
                colour = (byte)(150 - (Mathf.RoundToInt(gameObject.GetComponent<Block>().hitsRemaining / 50) * 20)); //50 points, 20 colour change

                gameObject.GetComponent<Block>().colour = new Color32(0, (byte)Mathf.Clamp((150 - gameObject.GetComponent<Block>().hitsRemaining), 0, 150), 255, 255);
                gameObject.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<Block>().colour;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Flas as used, to be deleted at end of shot.
        used = true;

        //turn line off
        points[4] = points[3] = points[2] = points[1] = points[0]=new Vector3(0, 0, 0);

        lRend.SetPositions(points);
    }
    
    public IEnumerator BlockDeath(GameObject blockHit, int x)
    {
        //GameManager.manager.actualNumberOfBlocks--;
        if (blockHit != null)
        {
            GameManager.manager.actualNumberOfBlocks--;
            Instantiate(explode, blockHit.transform.localPosition, Quaternion.identity);
            Destroy(blockHit);
            LevelGenerator.levelGenerator.block[x, yPos] = null;
        }
        yield return null;
    }
    
    public void Line()
    {
        lRend.positionCount = pointsCount;
        lRend.startWidth = 2f;
        lRend.endWidth = 2f;
        lRend.material = lightningMaterial;
    
        points[0] = new Vector3(-(GameManager.manager.camX / 2), transform.localPosition.y, transform.localPosition.z - 1);
        points[4] = new Vector3((GameManager.manager.camX / 2), transform.localPosition.y, transform.localPosition.z - 1);
        points[2] = GetCenter(points[0], points[4]);
        points[1] = GetCenter(points[0], points[2]);
        points[3] = GetCenter(points[2], points[4]);

        SetRandomness();

        lRend.SetPositions(points);
    }
    private void SetRandomness()
    {
        float randomness = 0.15f;

        for (int i = 0; i < points.Length; i++)
        {
            if (i != 0 && i != 4)
            {
                points[i].x += Random.Range(-randomness, randomness);
                points[i].y += Random.Range(-randomness, randomness);
                //points[i].z = -1;
            }
        }
    }

    private Vector3 GetCenter(Vector3 a, Vector3 b)
    {
        return (a + b) / 2;
    }

    //flash colour on hit
    IEnumerator FlashBlock(GameObject blockHit)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
        float flashTime = .15f;

        if (blockHit != null)
        {
            SpriteRenderer block = blockHit.GetComponent<SpriteRenderer>();
            while (elapsedTime < flashTime)
            {
                if(blockHit != null)
                {
                    block.color = Color.white; //red
                    yield return new WaitForSeconds(0.05f);

                    if (blockHit!=null)
                    {
                        //block.color = Color.white;
                    }
                    yield return new WaitForSeconds(0.05f);

                    if (blockHit!=null)
                    {
                        elapsedTime = Time.time - startTime;
                    }
                    else
                    {
                        elapsedTime += 2;//to end the whlie loop
                    }
                }
            }

            // Have to use this as if you store the colour of the block, if might already be white from being hit previously
            if(blockHit!=null)
                block.color = blockHit.GetComponentInParent<Block>().colour;        
        }
    }


}

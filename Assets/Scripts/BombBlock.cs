using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombBlock : MonoBehaviour
{

    public LevelGenerator levelGenerator;

    //public Electricity electricity
    public GameObject bombEmpty;

    public PlayLevel pl;

    [HideInInspector]
    public TextMeshProUGUI hitsRemainingText;

    public GameObject explode;

    float deathDelay = 2f;

    private void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("playLevel").GetComponent<LevelGenerator>();
        //playLevel = GameObject.FindGameObjectWithTag("playLevel").GetComponent<PlayLevel>();
    }

    private void OnDestroy()
    {
        //stop waiting for bomb activity
        GameManager.manager.keepWaiting = false;
        print("Bomb destroyed");

        GameObject[] m = GameObject.FindGameObjectsWithTag("message");
        foreach(GameObject message in m)
        {
            Destroy(message);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        GameManager.manager.keepWaiting = true;

        Collider2D col = gameObject.GetComponent<Collider2D>();
        
        if (col.isTrigger == true)
        {
            //prevent it being hit again
            col.isTrigger = false;
            col.enabled = false;

            //Bomb Sound and bomb explode
            AudioSource.PlayClipAtPoint(GameManager.manager.bombSound, gameObject.transform.localPosition, 100);

            StartCoroutine(Bomb());

            //Hide and then destroy
            //gameObject.GetComponent<Renderer>().enabled = false;
            Invoke("BombExplode", deathDelay);
            //Instantiate(explode, transform.localPosition, Quaternion.identity);
            Destroy(gameObject, deathDelay);

        }
    }

    void BombExplode()
    {
        Instantiate(explode, transform.localPosition, Quaternion.identity);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
            if ((!oldColour.Equals(orange)) && (!oldColour.Equals(red)) && (gameObject.tag != "special") && (gameObject.tag != "solidBlock") && (gameObject.tag != "bomb"))
            {
                colour = (byte)(150 - (Mathf.RoundToInt(gameObject.GetComponent<Block>().hitsRemaining / 50) * 20)); //50 points, 20 colour change

                gameObject.GetComponent<Block>().colour = new Color32(0, (byte)Mathf.Clamp((150 - gameObject.GetComponent<Block>().hitsRemaining), 0, 150), 255, 255);
                gameObject.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<Block>().colour;
            }
        }
    }

    IEnumerator Bomb()
    {
        int counter = 1;
        int reduction;
        int blocksToHitModulus = 2; // half the blocks
        float minHitsDivider = 2; //3
        float maxHitsDivider = 0.55f; // 0.75

        GameObject[] block = GameObject.FindGameObjectsWithTag("block"); // Not including super blocks...should I?
        foreach (GameObject b in block)
        {
            //should I do this block?
            if ((b != null) && (counter % blocksToHitModulus == 0))
            {
                //remove points depending on min and max hit dividers
                reduction = (int)Mathf.Round(Random.Range(b.GetComponentInParent<Block>().hitsRemaining / minHitsDivider, b.GetComponentInParent<Block>().hitsRemaining / maxHitsDivider));
                //update score
                if (reduction > b.GetComponentInParent<Block>().hitsRemaining)
                {
                    GameManager.manager.level[GameManager.manager.currentLevel].shotPoints += b.GetComponentInParent<Block>().hitsRemaining;
                }
                else
                {
                    GameManager.manager.level[GameManager.manager.currentLevel].shotPoints += reduction;
                }
                //reduce block hits remaining
                b.GetComponentInParent<Block>().hitsRemaining -= reduction;
                //Adjust hitsRemainingText
                if (b.GetComponentInParent<Block>().hitsRemaining > 0)
                {
                    //StartCoroutine(FlashBlock(b)); //flash the blocks
                    hitsRemainingText = b.GetComponentInChildren<TextMeshProUGUI>();
                    hitsRemainingText.text = b.GetComponentInParent<Block>().hitsRemaining.ToString();
                    //message showing number of hits needed reduced
                    StartCoroutine(GameManager.manager.Message("-" + reduction, b.transform.position, 4, 2, Color.white));

                    if (b != null)
                        BlockColour(b);
                }
                else
                {
                    hitsRemainingText = b.GetComponentInChildren<TextMeshProUGUI>();
                    hitsRemainingText.text = "0";
                    StartCoroutine(GameManager.manager.Message("X", b.transform.position, 4, 2, Color.white));
                    StartCoroutine(BlockDeath(b));
                }
                //Create new electricity line
                GameObject currentBomb = Instantiate(bombEmpty, transform.localPosition, Quaternion.identity);
                Electricity electricity = currentBomb.GetComponentInChildren<Electricity>();
                electricity.lines(b.transform.localPosition);
                Destroy(currentBomb, deathDelay);
            }
            counter++;
		}
        yield return null;
	}

	//flash colour on hit
    IEnumerator FlashBlock(GameObject blockHit)
    {
        float startTime = Time.time;
        float elapsedTime = 0;
        float flashTime = 2;

        if (blockHit != null)
        {
            SpriteRenderer block = blockHit.GetComponent<SpriteRenderer>();
            while (elapsedTime < flashTime)
            {
                if (blockHit.GetComponent<SpriteRenderer>() != null)
                {
                    block.color = Color.red;
                    yield return new WaitForSeconds(0.051f);
                    if (blockHit.GetComponent<SpriteRenderer>() != null)
                    {
                        block.color = Color.white;
                    }
                    yield return new WaitForSeconds(0.051f);
                    if (blockHit.GetComponent<SpriteRenderer>() != null)
                    {
                        elapsedTime = Time.time - startTime;
                    }
                }
            }

            // Have to use this as if you store the colour of the block, if might already be white from being hit previously
            block.color = blockHit.GetComponentInParent<Block>().colour;
        }
    }

    //Block death
    public IEnumerator BlockDeath(GameObject blockHit)
    {
        if (blockHit != null)
        {
            GameManager.manager.actualNumberOfBlocks--;
            Instantiate(explode, blockHit.transform.localPosition, Quaternion.identity);
            Destroy(blockHit, 2);
        }
        yield return null;
    }

    /*
    private void Update()
    {
        if(GameManager.manager.redo==true)
        {

        }
    }
    */
}


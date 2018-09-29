using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombBlock : MonoBehaviour {

    //public PlayLevel playLevel;
    //public Electricity electricity;
    public GameObject bombEmpty;

    [HideInInspector]
    public TextMeshProUGUI hitsRemainingText;

    public GameObject explode;

    float deathDelay = 1.5f;
    int blocksToHitModulus = 3;

    private void OnDestroy()
    {
        GameManager.manager.keepWaiting = false;    
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
    }

    
    IEnumerator Bomb()
    {
        int counter = 1;
        int currentHitBlock = 0;
        int reduction;

        GameObject[] block = GameObject.FindGameObjectsWithTag("block"); // Not including super blocks...should I?
        foreach (GameObject b in block)
        {
            //should I do this block?
            if ((b != null) && (counter % blocksToHitModulus == 0))
            {
                reduction = (int)Mathf.Round(Random.Range(b.GetComponentInParent<Block>().hitsRemaining / 3, b.GetComponentInParent<Block>().hitsRemaining * 1.1f));
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
                    StartCoroutine(GameManager.manager.Message("-" + reduction, b.transform.position, 3, deathDelay, Color.red));
                }
                else
                {
                    hitsRemainingText = b.GetComponentInChildren<TextMeshProUGUI>();
                    hitsRemainingText.text = "0";
                    StartCoroutine(GameManager.manager.Message("X", b.transform.position, 3, deathDelay, Color.red));
                    StartCoroutine(BlockDeath(b));
                }

                //Create new bomb
                GameObject currentBomb = Instantiate(bombEmpty, transform.localPosition, Quaternion.identity);
                Electricity electricity = currentBomb.GetComponentInChildren<Electricity>();
                electricity.lines(b.transform.localPosition);
                Destroy(currentBomb, deathDelay);

                currentHitBlock++;
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
        /*
        float deathTime = 0.25f;
        float elapsedTime = 0;
        Vector2 startingScale = gameObject.transform.localScale * 2.5f;
        Vector2 endingScale = new Vector2(0, 0);

        AudioSource.PlayClipAtPoint(GameManager.manager.blockDieSound, new Vector3(0, 0, 0));

        SpriteRenderer block = blockHit.GetComponent<SpriteRenderer>();

        while (elapsedTime < deathTime)
        {
            if (blockHit != null)
            {
                blockHit.transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / deathTime));
                elapsedTime += Time.deltaTime;
                block.color = Color.red;
                yield return null;
            }
        }

        //reduce number of blocks
        GameManager.manager.actualNumberOfBlocks--;

        block.enabled = false;
        if (blockHit != null)
        {
            Destroy(blockHit);
        }
        //Destroy(blockHit, deathDelay);
        */
        GameManager.manager.actualNumberOfBlocks--;
        if (blockHit != null)
        {
            Instantiate(explode, blockHit.transform.localPosition, Quaternion.identity);
            Destroy(blockHit);
        }
        yield return null;
        
    }
    
}


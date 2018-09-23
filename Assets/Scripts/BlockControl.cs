using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
 * MAYBE TRY TO PUT THE COLLIDER FUNCTION STUFF ON THE BALLS INSTEAD. SEE IF THAT STOPS THE RANDOM BOUNCES
 */


public class BlockControl : MonoBehaviour
{
    static public BlockControl blockControl;
    LevelGenerator levelGenerator;

    //Text hitsRemainingText;
    TextMeshProUGUI hitsRemainingText;

    Collider2D c;

    //reduce hits remaining and if needed, reduce number of blocks and delete object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //update current shot score (should be set to zero on ball launch in play level)
        GameManager.manager.level[GameManager.manager.currentLevel].shotPoints++;
        
        //put in some kind of flash when it is hit
        StartCoroutine(Flash());

        //reduce hits remaining
        gameObject.GetComponentInParent<Block>().hitsRemaining--;

        //check if block has been destroyed
        if (gameObject.GetComponentInParent<Block>().hitsRemaining == 0)
        {
            //turn off collider
            c = gameObject.GetComponentInChildren<Collider2D>();
            c.enabled = false;// !c.enabled;

            hitsRemainingText = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(); // get the textmeshpro element of the letterText
            hitsRemainingText.text = "0";// gameObject.GetComponentInParent<Block>().hitsRemaining.ToString();

            StartCoroutine(BlockDeath());
        }
        else if (GameManager.manager.currentLevel % 10 != 0)    //show hits remaining on all levels except the 'bonus' levels --- every 10th level.
        {
            hitsRemainingText = gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(); // get the textmeshpro element of the letterText
            hitsRemainingText.text = gameObject.GetComponentInParent<Block>().hitsRemaining.ToString();
        }


    }

    // This is used when invincible balls is active
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //turn off collider
        c = gameObject.GetComponentInChildren<Collider2D>();
        c.enabled = !c.enabled;

        GameManager.manager.level[GameManager.manager.currentLevel].shotPoints += gameObject.GetComponentInParent<Block>().hitsRemaining;
        StartCoroutine(BlockDeath());
        
    }

    //flash colour on hit
    IEnumerator Flash()
    {
        SpriteRenderer block = gameObject.GetComponent<SpriteRenderer>();
        block.color = Color.white;
        yield return new WaitForSeconds(0.025f);
        // Have to use this as if you store the colour of the block, if might already be white from being hit previously
        block.color = gameObject.GetComponentInParent<Block>().colour;
    }

    //Block death
    public IEnumerator BlockDeath()
    {
        float deathTime = 0.25f;
        float elapsedTime = 0;
        Vector2 startingScale = gameObject.transform.localScale*1.5f;
        Vector2 endingScale = new Vector2(0, 0);

        SpriteRenderer block = gameObject.GetComponent<SpriteRenderer>();
        
        while (elapsedTime < deathTime)
        {
            transform.localScale = Vector2.Lerp(startingScale, endingScale, (elapsedTime / deathTime));
            elapsedTime += Time.deltaTime;
            block.color = Color.red;
            yield return null;
        }

        //reduce number of blocks
        GameManager.manager.actualNumberOfBlocks--;

        Destroy(gameObject);
    }
}
	


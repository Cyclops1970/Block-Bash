using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockReduction : MonoBehaviour
{
    public PlayLevel playLevel;
    public BlockControl blockControl;
    public GameObject shopPanel;
    public bool blockReductionActive;
    public GameObject explosion;

    TextMeshProUGUI hitsRemainingText;

    public void ReduceBlockHitsToKill()
    {
        if (GameManager.manager.numberOfBockReductions > 0)
        {
            if (blockReductionActive == false)
            {
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.purchaseSound, Camera.main.transform.position);

                blockReductionActive = true;

                //reduce number of powerups left
                GameManager.manager.numberOfBockReductions--;
                PlayerPrefs.SetInt(GameManager.manager.blockReduction, GameManager.manager.numberOfBockReductions);

                //Change the totalLevelPoints Left to adjust reward bar
                GameManager.manager.totalLevelPoints = 0;

                // find all the blocks and reduce the hitsremaining
                GameObject[] block = GameObject.FindGameObjectsWithTag("block");
                foreach (GameObject b in block)
                {
                    if ((b != null) && (b.gameObject.GetComponentInParent<Block>().hitsRemaining != 1))
                    {
                        int newHitsRemaining = (int)(b.gameObject.GetComponentInParent<Block>().hitsRemaining * 0.75f);
                        int reduction = b.gameObject.GetComponentInParent<Block>().hitsRemaining - newHitsRemaining;

                        b.gameObject.GetComponentInParent<Block>().hitsRemaining = newHitsRemaining; // * 75% to remove 25%

                        hitsRemainingText = b.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(); // get the textmeshpro element of the letterText
                        hitsRemainingText.text = b.gameObject.GetComponentInParent<Block>().hitsRemaining.ToString();

                        //message showing number of hits needed reduced
                        //StartCoroutine(GameManager.manager.Message("-" + reduction, b.transform.position, 4, 1.5f, Color.white));
                        Instantiate(explosion, b.transform.localPosition, Quaternion.identity);
                        
                        //update total level points 
                        GameManager.manager.totalLevelPoints += b.GetComponentInParent<Block>().hitsRemaining;

                        AudioSource.PlayClipAtPoint(GameManager.manager.blockReductionSound, Camera.main.transform.position);
                    }
                }
                

                StartCoroutine(GameManager.manager.Message("Hits Needed Reduced!", new Vector2(0, 0), 8, 1.5f, Color.white));
            }
        }
        else if(blockReductionActive == false)
        {
            if (GameManager.manager.playerCoins >= GameManager.manager.blockReductionCost)
            {
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.purchaseSound, Camera.main.transform.position);

                //take the cost of the powerup from player coins and update number of powerups available
                GameManager.manager.playerCoins -= GameManager.manager.blockReductionCost;
                PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);
                GameManager.manager.numberOfBockReductions++;
                PlayerPrefs.SetInt(GameManager.manager.blockReduction, 1);
                StartCoroutine(GameManager.manager.Message("Purchased" + "\r\n" + "Block Reduction", new Vector2(0, 0), 8, 1.5f, Color.white));
            }
            else
            {
                //Open the shop so they can buy stuff
                shopPanel.SetActive(true);
            }
        }
        
    }

    IEnumerator Reduction()
    {
        yield return null;
    }

}

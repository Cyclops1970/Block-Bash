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

    TextMeshProUGUI hitsRemainingText;

    public void ReduceBlockHitsToKill()
    {
        if (GameManager.manager.numberOfBockReductions > 0)
        {
            if (blockReductionActive == false)
            {
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
                        b.gameObject.GetComponentInParent<Block>().hitsRemaining = (int)(b.gameObject.GetComponentInParent<Block>().hitsRemaining * 0.75f); // * 75% to remove 25%

                        hitsRemainingText = b.gameObject.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>(); // get the textmeshpro element of the letterText
                        hitsRemainingText.text = b.gameObject.GetComponentInParent<Block>().hitsRemaining.ToString();

                        //update total level points 
                        GameManager.manager.totalLevelPoints += b.GetComponentInParent<Block>().hitsRemaining;
                    }
                }

                StartCoroutine(GameManager.manager.Message("Hits Needed Reduced!"));
            }
        }
        else
        {
            if (GameManager.manager.playerCoins >= GameManager.manager.blockReductionCost)
            {
                //take the cost of the powerup from player coins and update number of powerups available
                GameManager.manager.playerCoins -= GameManager.manager.blockReductionCost;
                GameManager.manager.numberOfBockReductions++;
                StartCoroutine(GameManager.manager.Message("Purchased" + "\r\n" + "Block Reduction"));
            }
            else
            {
                //Open the shop so they can buy stuff
                shopPanel.active = true;
            }
        }
        
    }

}

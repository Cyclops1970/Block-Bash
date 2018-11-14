using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Balls2x : MonoBehaviour
{
    [HideInInspector]
    static public Balls2x balls2x;
    [HideInInspector]
    public bool doubleBalls;

    public GameObject shopPanel;

    //int currentShot;

    public void DoubleBalls()
    {
        if (GameManager.manager.numberOfBalls2x > 0)
        {
            if (doubleBalls == false) // if Not already active
            {
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.purchaseSound, Camera.main.transform.position);

                doubleBalls = true;

                GameManager.manager.maxNumberOfBalls *= 2;
                GameManager.manager.currentNumberOfBalls = GameManager.manager.maxNumberOfBalls;

                StartCoroutine(GameManager.manager.Message("Double Balls!" + "\r\n" + "(" + GameManager.manager.maxNumberOfBalls + ")", new Vector3(0,0,0), 8, 1.5f, Color.white));

                //reduce number of powerups left
                GameManager.manager.numberOfBalls2x--;
                PlayerPrefs.SetInt(GameManager.manager.balls2x, GameManager.manager.numberOfBalls2x);

                //maybe make the button inactive??
            }
        }
        else if(doubleBalls==false)
        {
            if(GameManager.manager.playerCoins >= GameManager.manager.balls2xCost)
            {
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.purchaseSound, Camera.main.transform.position);

                //take the cost of the powerup from player coins and update number of powerups available
                GameManager.manager.playerCoins -= GameManager.manager.balls2xCost;
                PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);
                GameManager.manager.numberOfBalls2x++;
                StartCoroutine(GameManager.manager.Message("Purchased"+"\r\n"+"Double Balls", new Vector2(0, 0), 8, 1.5f, Color.white));
            }
            else
            {
                //Open the shop so they can buy stuff
                shopPanel.active = true;
            }
        }
    }

  

}

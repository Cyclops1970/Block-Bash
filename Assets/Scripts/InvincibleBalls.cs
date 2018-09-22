﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBalls : MonoBehaviour
{ 
    [HideInInspector]
    static public InvincibleBalls invincibleBalls;
    //[HideInInspector]
    public bool invincibleBallsActive;

    public GameObject shopPanel;

    public void Invincible()
    {
        if (GameManager.manager.numberOfInvincibleBalls > 0)
        {
            if (invincibleBallsActive == false)
            {
                invincibleBallsActive = true;

                StartCoroutine(GameManager.manager.Message("Power Balls!"));

                //reduce number of powerups left
                GameManager.manager.numberOfInvincibleBalls--;
                PlayerPrefs.SetInt(GameManager.manager.invincibleBalls, GameManager.manager.numberOfInvincibleBalls);

                //Set all blocks to just be triggers
                GameObject[] blocks = GameObject.FindGameObjectsWithTag("block");
                foreach (GameObject b in blocks)
                {
                    b.GetComponentInChildren<Collider2D>().isTrigger = true;
                }

                //maybe make the button inactive??
            }
        }
        else
        {
            if (GameManager.manager.playerCoins >= GameManager.manager.invincibleBallsCost)
            {
                //take the cost of the powerup from player coins and update number of powerups available
                GameManager.manager.playerCoins -= GameManager.manager.invincibleBallsCost;
                GameManager.manager.numberOfInvincibleBalls++;
                StartCoroutine(GameManager.manager.Message("Purchased" + "\r\n" + "Power Balls!"));
            }
            else
            {
                //Open the shop so they can buy stuff
                shopPanel.active = true;
            }
        }
    }

}


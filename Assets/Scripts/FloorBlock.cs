﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorBlock : MonoBehaviour {

    public LevelGenerator levelGenerator;

    [HideInInspector]
    static public FloorBlock floorBlock;
    public GameObject FloorBlocker;
    public GameObject shopPanel;
    public bool floorBlocksActive;

    public void PlaceFloorBlocks()
    {
        if (GameManager.manager.numberOfFloorBlocks > 0)
        {
            if (floorBlocksActive == false)
            {
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.purchaseSound, Camera.main.transform.position);

                floorBlocksActive = true;
                GameManager.manager.numberOfFloorBlocks--;
                PlayerPrefs.SetInt(GameManager.manager.floorBlock, GameManager.manager.numberOfFloorBlocks);

                //set correct number of floor blocks to be depending of if the level width is odd or even
                int numberOfBlocks = levelGenerator.currentLevel.width;
                if (numberOfBlocks % 2 != 0)
                {
                    numberOfBlocks = (numberOfBlocks / 2) + 1;
                }
                else
                {
                    numberOfBlocks = numberOfBlocks / 2;
                }

                // get starting X and Y pos
                float xStart = (-GameManager.manager.camX / 2 + levelGenerator.blockScaleAdjustedX / 2);
                float yPos = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (levelGenerator.blockScaleAdjustedY / 2);
                //place blocks
                for (int x = 0; x < numberOfBlocks; x++)
                {
                    GameObject b = Instantiate(FloorBlocker);
                    b.GetComponentInChildren<Block>().transform.localScale = new Vector2(levelGenerator.blockScaleAdjustedX, levelGenerator.blockScaleAdjustedY);
                    b.GetComponentInChildren<Block>().transform.localPosition = new Vector3(xStart + ((x * 2) * levelGenerator.blockScaleAdjustedX), yPos, 10);
                }
                StartCoroutine(GameManager.manager.Message("Floor Blocks!", new Vector2(0, 0), 8, 1.5f, Color.white));
            }
        }
        else if(floorBlocksActive == false)
        {
            if (GameManager.manager.playerCoins >= GameManager.manager.floorBlockCost)
            {
                //sound
                AudioSource.PlayClipAtPoint(GameManager.manager.purchaseSound, Camera.main.transform.position);

                //take the cost of the powerup from player coins and update number of powerups available
                GameManager.manager.playerCoins -= GameManager.manager.floorBlockCost;
                PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);
                GameManager.manager.numberOfFloorBlocks++;
                PlayerPrefs.SetInt(GameManager.manager.floorBlock, 1);
                StartCoroutine(GameManager.manager.Message("Purchased" + "\r\n" + "Floor Blocks", new Vector2(0, 0), 8, 1.5f, Color.white));
            }
            else
            {
                //Open the shop so they can buy stuff
                shopPanel.SetActive(true);
            }
        }
    }
}


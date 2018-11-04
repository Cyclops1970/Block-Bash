using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour {

    public Button continueButton;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI blocksRemainingText;
    public GameObject bottomPanel;
    public GameObject dimCoinsNeeded;

    public PlayLevel playLevel;

    private void OnEnable()
    {
        //hide bottom panel 
        bottomPanel.SetActive(false);
        StartCoroutine(playLevel.ShotPanelHide());
        print("hidden in fail");

        //Show the number of blocks remaining
        blocksRemainingText.text = "You left " + GameManager.manager.actualNumberOfBlocks.ToString() + " blocks remaining!";

        //show the cost to continue
        costText.text = GameManager.manager.continueCost.ToString();

        //Set continue button to active or not depending on player coins
        GameManager.manager.playerCoins = PlayerPrefs.GetInt("playerCoins");

        //Give option to continue or not, depending on available coins
        if (GameManager.manager.playerCoins < GameManager.manager.continueCost)
        {
            continueButton.interactable = false;
            dimCoinsNeeded.SetActive(true);
        }
        else
        {
            continueButton.interactable = true;
            dimCoinsNeeded.SetActive(false);
        }
    }


    private void OnDisable()
    {
        //print("Fail Panel Disabled");
    }


}

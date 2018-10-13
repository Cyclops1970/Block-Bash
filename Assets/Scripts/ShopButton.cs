using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject dragCollider;
    public GameObject shopButton;
    public GameObject failPanel;
    public GameObject passPanel;

    static private bool fp;

    public void OpenShop()
    {
        fp = false;

        //Don't allow if balls active.  Maybe grey out, in playlevel

        //Make the shop panel active 
        if(!GameManager.manager.ballsActive)
        {
            if(failPanel.activeInHierarchy)
            {
                fp = true;
                failPanel.SetActive(false);
            }
            
            shopPanel.SetActive(true);

            //prevent balls from starting
            dragCollider.SetActive(false);

        }

    }

    public void CloseShop()
    {
        if (fp == true)
        {
            failPanel.SetActive(true);
        }

        shopPanel.SetActive(false);
        dragCollider.SetActive(true);
    }

    public void shopButtonInactive()
    {

    }
    public void shopButtonActive()
    {

    }
}

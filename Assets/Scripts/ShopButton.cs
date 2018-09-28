using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject dragCollider;
    public GameObject shopButton;

    public void OpenShop()
    {
        //Don't allow if balls active.  Maybe grey out, in playlevel

        //Make the shop panel active 
        if(!GameManager.manager.ballsActive)
        {
            shopPanel.SetActive(true);

            //prevent balls from starting
            dragCollider.SetActive(false);

        }

    }

    public void CloseShop()
    {
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

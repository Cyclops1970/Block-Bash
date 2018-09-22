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
            shopPanel.active = true;

            //prevent balls from starting
            dragCollider.active = false;

        }

    }

    public void CloseShop()
    {
        shopPanel.active = false;

        dragCollider.active = true;

    }

    public void shopButtonInactive()
    {

    }
    public void shopButtonActive()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour {

    public GameObject gameBottomPanel;
    bool shoot;

    //public GameObj
    private void OnEnable()
    {
        shoot = false;

        if(GameManager.manager.ballsActive == false)
        {
            GameManager.manager.ballsActive = true; //prevent playing starting a shot 
            shoot = true; // set so I know change the ballsactive back on disable.
        }
        gameBottomPanel.SetActive(false);
    }

    private void OnDisable()
    {
        if(shoot==true)
        {
            GameManager.manager.ballsActive = false;
            gameBottomPanel.SetActive(true);
        }
    }

}

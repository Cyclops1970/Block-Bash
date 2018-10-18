using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public GameObject settingsPanel;

    public void OpenSettingsMenu()
    {
        settingsPanel.SetActive(true);

        //Disable all the level buttons
        GameObject[] buttons =  GameObject.FindGameObjectsWithTag("levelButton");
        foreach(GameObject b in buttons)
        {
            b.GetComponent<Button>().interactable = false;
            
        }
    }

    public void CloseSettingsMenu()
    {
        settingsPanel.SetActive(false);

        //Enable all the level buttons
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("levelButton");
        foreach (GameObject b in buttons)
        {
            b.GetComponent<Button>().interactable = true;
        }
    }
}

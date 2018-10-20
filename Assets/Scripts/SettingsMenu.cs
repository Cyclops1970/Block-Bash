using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour {

    public GameObject settingsPanel;
    public TextMeshProUGUI versionText;

    public void OpenSettingsMenu()
    {
        settingsPanel.SetActive(true);

        //Disable all the level buttons
        GameObject[] buttons =  GameObject.FindGameObjectsWithTag("levelButton");
        foreach(GameObject b in buttons)
        {
            b.GetComponent<Button>().interactable = false;
            
        }

        versionText.text = "Version: " + Application.version;
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

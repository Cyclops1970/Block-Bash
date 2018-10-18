using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HowToPlay : MonoBehaviour
{
    public GameObject settingsPanel, howToPlayPanel;
    public GameObject[] panel;
    public TextMeshProUGUI pageNumberText;

    int pageNumber, totalNumberOfPages;
    
    private void Start()
    {
        pageNumber = 1;
        totalNumberOfPages = 10;

        pageNumberText.text = (pageNumber + " / " + totalNumberOfPages);
        panel[pageNumber - 1].SetActive(true);
    }

    public void nextPage()
    {
        if(pageNumber+1 <= totalNumberOfPages)
        {
            //Hide current panel
                

            pageNumber++;

            //update pagenumber text
            pageNumberText.text = (pageNumber + " / " + totalNumberOfPages);
        }
    }

    public void prevPage()
    {
        if(pageNumber-1 >= 1)
        {


            pageNumber--;

            //update pagenumber text
            pageNumberText.text = (pageNumber + " / " + totalNumberOfPages);
        }
    }

    public void CloseHowToPlayPanel()
    {
        //back to settings panel
        settingsPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }

    public void OpenHowToPlayPanel()
    {
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
    }

}

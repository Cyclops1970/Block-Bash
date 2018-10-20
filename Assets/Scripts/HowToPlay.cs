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
        pageNumber = 0;
        totalNumberOfPages = 5;

        panel[pageNumber].SetActive(true);

        ShowPageNumber();
    }

    private void ShowPageNumber()
    {
        //update pagenumber text
        pageNumberText.text = (pageNumber+1 + " / " + totalNumberOfPages);
    }

    public void nextPage()
    {
        if(pageNumber+1 < totalNumberOfPages)
        {
            //Hide current panel
            panel[pageNumber].SetActive(false);    

            pageNumber++;

            //Show next panel
            panel[pageNumber].SetActive(true);

            ShowPageNumber();
        }
    }

    public void prevPage()
    {
        if(pageNumber-1 >= 0)
        {
            //Hide current panel
            panel[pageNumber].SetActive(false);

            pageNumber--;

            //Show prev panel
            panel[pageNumber].SetActive(true);

            ShowPageNumber();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPanel : MonoBehaviour {
    static public ShopPanel shopPanel { get; set; }
    public ShopPacks shopPacks;
    public GameObject gameBottomPanel;
    public GameObject dragPanel;
    public TextMeshProUGUI twoDollarCoins;
    public TextMeshProUGUI fiveDollarCoins;
    public TextMeshProUGUI tenDollarCoins;
    public TextMeshProUGUI twentyFiveDollarCoins;
    public TextMeshProUGUI fiftyDollarCoins;


    private void Awake()
    {
        shopPanel = this;
    }

    private void Start()
    {
        //twoDollarCoins.text = shopPacks.twoDollarsCoins.ToString("n0");
        //fiveDollarCoins.text = shopPacks.fiveDollarsCoins.ToString("n0");
        //tenDollarCoins.text = shopPacks.tenDollarsCoins.ToString("n0");
        //twentyFiveDollarCoins.text = shopPacks.twentyFiveDollarsCoins.ToString("n0");
        //fiftyDollarCoins.text = shopPacks.fiftyDollarsCoins.ToString("n0");
    }
    private void OnEnable()
    {
        
        //Delete any messages on screen
        GameObject[] messages = GameObject.FindGameObjectsWithTag("message");
        foreach (GameObject m in messages)
            Destroy(m);
            
        gameBottomPanel.SetActive(false);
        dragPanel.SetActive(false);
    }

    private void OnDisable()
    {
        gameBottomPanel.SetActive(true);
        dragPanel.SetActive(true);
    }


}

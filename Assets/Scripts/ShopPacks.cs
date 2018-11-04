using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPacks : MonoBehaviour {

    static public ShopPacks shopPacks { get; set; }

    private void Awake()
    {
        shopPacks = this;
    }

    public void TwoDollars()
    {
        StartCoroutine(GameManager.manager.Message("2000 Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += 2000;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void FiveDollars()
    {
        StartCoroutine(GameManager.manager.Message("7000 Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += 7000;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void TenDollars()
    {
        StartCoroutine(GameManager.manager.Message("15,000 Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += 15000;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void TwentyFiveDollars()
    {
        StartCoroutine(GameManager.manager.Message("50,000 Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += 50000;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void FiftyDollars()
    {
        StartCoroutine(GameManager.manager.Message("100,000 Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += 100000;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
}

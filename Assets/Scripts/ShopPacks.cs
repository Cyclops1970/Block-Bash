using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPacks : MonoBehaviour {

    static public ShopPacks shopPacks { get; set; }

    public int twoDollarsCoins = 1500;
    public int fiveDollarsCoins = 3750;
    public int tenDollarsCoins = 8000;
    public int twentyFiveDollarsCoins = 20000;
    public int fiftyDollarsCoins = 50000;

    
    private void Awake()
    {
        shopPacks = this;
    }

    public void TwoDollars()
    {
        StartCoroutine(PlayCoinsSound());
        StartCoroutine(GameManager.manager.Message(twoDollarsCoins+" Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += twoDollarsCoins;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void FiveDollars()
    {
        StartCoroutine(PlayCoinsSound());
        StartCoroutine(GameManager.manager.Message(fiveDollarsCoins+" Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += fiveDollarsCoins;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void TenDollars()
    {
        StartCoroutine(PlayCoinsSound());
        StartCoroutine(GameManager.manager.Message(tenDollarsCoins+" Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += tenDollarsCoins;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void TwentyFiveDollars()
    {
        StartCoroutine(PlayCoinsSound());
        StartCoroutine(GameManager.manager.Message(twentyFiveDollarsCoins+" Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += twentyFiveDollarsCoins;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }
    public void FiftyDollars()
    {
        StartCoroutine(PlayCoinsSound());
        StartCoroutine(GameManager.manager.Message(fiftyDollarsCoins+" Coins!", Vector2.zero, 10, 2, Color.white));

        GameManager.manager.playerCoins += fiftyDollarsCoins;
        PlayerPrefs.SetInt("playerCoins", GameManager.manager.playerCoins);

        ShopButton.shopButton.CloseShop();
    }

    IEnumerator PlayCoinsSound()
    {
        for (int x = 1; x < 10; x++)
        {
            AudioSource.PlayClipAtPoint(GameManager.manager.coin, gameObject.transform.localPosition);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}

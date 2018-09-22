using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PassPanel : MonoBehaviour {

    public GameObject passFace;
    public Sprite neutralFace, smileFace, happyFace;
    public TextMeshProUGUI coinText;
    Image result;

    private void OnEnable()
    {
        result = passFace.GetComponent<Image>();
        
        if (GameManager.manager.currentLevelStars == 3)
        {
            result.sprite = happyFace;
            if(GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                coinText.text = (GameManager.manager.starCoins3*2).ToString();
            else
                coinText.text = GameManager.manager.starCoins3.ToString();
        }
        else if (GameManager.manager.currentLevelStars == 2)
        {
            result.sprite = smileFace;
            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                coinText.text = (GameManager.manager.starCoins2 * 2).ToString();
            else
                coinText.text = GameManager.manager.starCoins2.ToString();
        }
        else
        {
            result.sprite = neutralFace;
            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
                coinText.text = (GameManager.manager.starCoins1 * 2).ToString();
            else
                coinText.text = GameManager.manager.starCoins1.ToString();
            
        }


    }
}

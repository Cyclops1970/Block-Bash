using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    public GameObject background;

    // Use this for initialization
    void Start()
    {
        DisplayTitle();
        StartCoroutine(TitleToHome());
    }

    IEnumerator TitleToHome()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Home");

    }

    void DisplayTitle()
    {
        SpriteRenderer bkg = background.GetComponent<SpriteRenderer>();
        float camY = Camera.main.orthographicSize * 2;
        float camX = Camera.main.orthographicSize * 2 * Screen.width / Screen.height; // covers different aspectc ratios
        float sprX = bkg.sprite.bounds.size.x;
        float sprY = bkg.sprite.bounds.size.y;
        Vector2 scale = transform.localScale;

        scale.x = camX / sprX;
        scale.y = camY / sprY/2;

        bkg.transform.position = Vector2.zero; // Optional, place in middle
        bkg.transform.localScale = scale;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class Title : MonoBehaviour
{

    //public GameObject background;

    // Use this for initialization
    void Start()
    {
        //DisplayTitle();
        StartCoroutine(TitleToHome());
    }

    IEnumerator TitleToHome()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Home");

    }

    void DisplayTitle()
    {
        //Add VideoPlayer to the GameObject
        //videoPlayer = gameObject.AddComponent<VideoPlayer>();
        //Add AudioSource
        //audioSource = gameObject.AddComponent<AudioSource>();

        //Set video To Play then prepare Audio to prevent Buffering
        //videoPlayer.clip = movie;

        ///videoPlayer.source = VideoSource.VideoClip;
        
        //Assign the Texture from Video to RawImage to be displayed
        //image.texture = videoPlayer.texture;

        //Play Video
        //videoPlayer.Play();

        //Play Sound
        //audioSource.Play();

        /*
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
        */
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndShotSuggestion : MonoBehaviour {

    public Button endShotButton;

    private void Start()
    {
        StartCoroutine(EndShotAnim());
    }

    IEnumerator EndShotAnim()
    {
        float flashTime = 1;
        ColorBlock cb = endShotButton.colors;
        Color origColour = cb.normalColor;

        while (this.enabled==true)
        {
            if(Time.timeScale > 5)
            {
                cb.normalColor = Color.red;
                endShotButton.colors = cb;
                yield return new WaitForSecondsRealtime(flashTime);
                cb.normalColor = origColour;
                endShotButton.colors = cb;
                yield return new WaitForSecondsRealtime(flashTime);
            }
            yield return null;
        }

    }
}

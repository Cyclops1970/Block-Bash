using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NeedAPowerUp : MonoBehaviour {

    static public NeedAPowerUp needAPowerUp;
    public PlayLevel playLevel;

    public GameObject powerUpPanel;
    public TextMeshProUGUI powerUpQuestionText;
    public Collider2D screenCollider;

    string[] powerUpQuestion = new string[] { "Maybe you need a powerup?", "Powerup time?", "You seem to be stuck, how about a powerup?", "Try, try again, or you could use a powerup?",
                                              "I have a strong feeling of Deja Vu!. Powerup?", "You must like this level, you keep trying it again and again. What about a powerup?",
                                              "Are you allergic to using powerups?", "Persistance is the key, powerups are quicker!", "Hello old chap, might I suggest a powerup?",
                                              "I don't want you to get too frustrated.  Maybe try a powerup so you don't break your phone?"};

    public void TimeForPowerUp()
    {
        screenCollider.enabled = false;
        powerUpPanel.SetActive(true);
        powerUpQuestionText.text = powerUpQuestion[Random.Range(0, powerUpQuestion.Length)];
    }

    public void ClosePowerUpPanel()
    {
        powerUpPanel.SetActive(false);
        screenCollider.enabled = true;
    }
}

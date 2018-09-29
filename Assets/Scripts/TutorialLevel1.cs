using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel1 : MonoBehaviour {

    static public TutorialLevel1 t1;
    public PlayLevel playLevel;

    public void Tutorial()
    {
        print("Hello");
        StartCoroutine(playLevel.Play());
    }

}

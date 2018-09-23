using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BordersHit : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(GameManager.manager.wallHitSound, new Vector3(0, 0, 0));
    }
}

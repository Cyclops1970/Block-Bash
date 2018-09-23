using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidBlockHit : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(GameManager.manager.solidHitSound, gameObject.transform.localPosition);
    }
}

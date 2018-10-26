using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneShake : MonoBehaviour
{
    //public PhoneShake phoneShake;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
    float lowPassKernelWidthInSeconds = 1.0f;
    // This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
    float shakeDetectionThreshold = 4.0f;

    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
    private Vector3 acceleration;
    private Vector3 deltaAcceleration;

    [HideInInspector]
    public int shakeCount = 0;
    
    void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }

    void Update()
    {
        acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        deltaAcceleration = acceleration - lowPassValue;

        if(((deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold) && (shakeCount < 1) && GameManager.manager.ballsActive==true))
        {
            shakeCount++;
            
            StartCoroutine(GameManager.manager.Message("You shouldn't shake your phone!", Vector3.zero, 8, 1, Color.white));

            GameObject[] block = GameObject.FindGameObjectsWithTag("block");
            foreach(GameObject b in block)
            {
                b.transform.localRotation = Random.rotation;
            }
            
            // Perform your "shaking actions" here, with suitable guards in the if check above, if necessary to not, to not fire again if they're already being performed.
            AudioSource.PlayClipAtPoint(GameManager.manager.undoShot, Vector2.zero);
        }
    }
}


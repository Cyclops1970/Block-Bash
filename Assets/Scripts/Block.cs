using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour //: Component //: MonoBehaviour
{
    [HideInInspector]
    public bool active;
    [HideInInspector]
    public int hitsRemaining;
    [HideInInspector]
    public Color32 colour;
    [HideInInspector]
    public enum type {square, circle, angle};
}



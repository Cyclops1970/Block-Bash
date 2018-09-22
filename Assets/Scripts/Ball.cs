using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ball 
{
    public enum TypeOfBall { normal, doubleStrength, invincible, blockKiller };
    
    public TypeOfBall ballType;
    public bool active;
    public Vector2 position;
    public GameObject ball;
}

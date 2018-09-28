using UnityEngine;
using System.Collections;

public class Electricity : MonoBehaviour
{
    //static public Electricity electricity;

    private LineRenderer lRend;
    private Vector3 transformPointA;
    private Vector3 transformPointB;

    private readonly int pointsCount = 5;
    private readonly int half = 2;
    private float randomness;
    private Vector3[] points;

    private readonly int pointIndexA = 0;
    private readonly int pointIndexB = 1;
    private readonly int pointIndexC = 2;
    private readonly int pointIndexD = 3;
    private readonly int pointIndexE = 4;

    public Material lightningMaterial;

    //private readonly string mainTexture = "_MainTex";
    private readonly string mainTexture = "_MainTex";
    private Vector2 mainTextureScale = Vector2.one;
    private Vector2 mainTextureOffset = Vector2.one;

    private float timer;
    private float timerTimeOut = 0.05f;
    private float lineSize = 1.5f;

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        CalculatePoints();
    }

    public void lines(Vector3 block)
    {
        gameObject.AddComponent<LineRenderer>();
        lRend = GetComponent<LineRenderer>();
        points = new Vector3[pointsCount];
        lRend.positionCount = pointsCount;
        lRend.startWidth = lineSize;
        lRend.endWidth = lineSize;
        lRend.material = lightningMaterial;

        transformPointB = block;
        enabled = true;
    }

    private void CalculatePoints()
    {
        timer += Time.deltaTime;

        if (timer > timerTimeOut) 
        {
            timer = 0;

            points[pointIndexA] = transformPointA = gameObject.transform.position;
            points[pointIndexE] = transformPointB;
            points[pointIndexC] = GetCenter(points[pointIndexA], points[pointIndexE]);
            points[pointIndexB] = GetCenter(points[pointIndexA], points[pointIndexC]);
            points[pointIndexD] = GetCenter(points[pointIndexC], points[pointIndexE]);

            float distance = Vector3.Distance(transformPointA, transformPointB) / points.Length; //evenly spaces direction change
            mainTextureScale.x = distance;
            mainTextureOffset.x = Random.Range(-randomness, randomness);
            lRend.material.SetTextureScale(mainTexture, mainTextureScale);
            lRend.material.SetTextureOffset(mainTexture, mainTextureOffset);

            randomness = distance / (pointsCount * half);

            SetRandomness();

            lRend.SetPositions(points);
        }
    }

    private void SetRandomness()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i != pointIndexA && i != pointIndexE)
            {
                points[i].x += Random.Range(-randomness, randomness);
                points[i].y += Random.Range(-randomness, randomness);
                points[i].z = -1;
            }
        }
    }

    private Vector3 GetCenter(Vector3 a, Vector3 b)
    {
        return (a + b) / half;
    }
}

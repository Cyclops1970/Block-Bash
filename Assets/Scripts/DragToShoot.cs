using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragToShoot : MonoBehaviour
{
    // This is attached to the fullscreen collider

    public PlayLevel playLevel;
    public LevelGenerator levelGenerator;
    public FloorBlock floorBlock;

    bool lineStart = false;
    Vector3[] linePoints;

    Camera camera;
    LineRenderer lineRenderer;
    public Material lineMaterial;

    // prevent angle allowing a whole row to be knocked out
    float minAngle = 0.5f; //0.5
    
    //[Header("Sprite for the ball")]
    public GameObject ballSprite;

    float zPos = -1f;

    Vector3 direction;
    Vector3 position;
    Vector3 staticPosition;
    Vector3 staticDirection;

    private void Start()
    {
        camera = FindObjectOfType<Camera>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.05f;
        linePoints = new Vector3[2];
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.02f;

        lineRenderer.material = lineMaterial;
        //lineRenderer.material = new Material(Shader.Find("Unlit/Texture"));
    }

    private void OnMouseDown()
    {
        if ((GameManager.manager.ballsActive == false) && (playLevel.bottomReached == false))
        {
            lineRenderer.positionCount = 2; // two points to draw line, start and end
            
            linePoints[0] = camera.ScreenToWorldPoint(Input.mousePosition);
            linePoints[0].z = zPos;

            //Adjust starting height to compenstate for the floor blocks if active
            if (floorBlock.floorBlocksActive == true)
            {
                linePoints[0].y = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (ballSprite.transform.localScale.y / 5.5f) + levelGenerator.blockScaleAdjustedY;
            }
            else
            {
                linePoints[0].y = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (ballSprite.transform.localScale.y / 5.5f);
            }

            lineStart = true;

            playLevel.startPos = Input.mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if ((GameManager.manager.ballsActive == false) && (lineStart == true))
        {   
            playLevel.endPos = Input.mousePosition;

            //ensure only shoot upwards
            if (camera.ScreenToWorldPoint(Input.mousePosition).y > linePoints[0].y+minAngle)
            {
                playLevel.ableToShoot = true;
            }

            lineStart = false;
            //hide line on mouse release
            lineRenderer.positionCount = 0;

        }
    }

    
    private void FixedUpdate()
    {
        //draw the rubber band line
        if (lineStart == true)
        {

            //set colour -- red for can't shoot, green for can shoot
            //ensure only shoot upwards
            if (camera.ScreenToWorldPoint(Input.mousePosition).y <= linePoints[0].y + minAngle)
            {
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
            }
            else
            {
                lineRenderer.startColor = Color.green;
                lineRenderer.endColor = Color.green;
            }

            linePoints[1] = camera.ScreenToWorldPoint(Input.mousePosition);
            linePoints[1].z = zPos;
            lineRenderer.SetPositions(linePoints);
        }
    }   
}

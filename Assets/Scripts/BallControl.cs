using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    //[Header("Game Manager")]
    //public GameManager m;
    public static BallControl ballControl;
    public LevelGenerator levelGenerator;
    public FloorBlock floorBlock;
    public InvincibleBalls invincibleBalls;
    public Balls2x balls2x;

    Camera camera;

    //Max number of balls
    //[HideInInspector]
    //public int maxNumOfBalls;
    //[HideInInspector]
    //public int currentNumberOfBalls;

    [HideInInspector]
    public Ball[] balls;

    [Header("Sprite for the ball")]
    public GameObject ballSprite;

    [HideInInspector]
    public float speed, timeBetweenBalls;

    float nextSpeedUpdate, speedUpdateTime, speedIncrease, maxSpeed;

    float xPos, yPos;
    GameObject ballContainer;

    public void CalculateNumberOfBalls()
    {
        //Set the number of balls according to current level;
        GameManager.manager.maxNumberOfBalls = GameManager.manager.baseNumberOfBalls + (int)(Mathf.Round((GameManager.manager.currentLevel / 5)));
        
        //Double balls for bonus level
        if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
        {
            GameManager.manager.maxNumberOfBalls *= 2;
        }
        if(balls2x.doubleBalls==true)
        {
            GameManager.manager.maxNumberOfBalls *= 2;
        }
        GameManager.manager.currentNumberOfBalls = GameManager.manager.maxNumberOfBalls;
    }

    public void InitializeBalls(Vector3 start)
    {
        Vector3 pos;

        Time.timeScale = 1;

        camera = FindObjectOfType<Camera>();

        speed = 7;
        timeBetweenBalls = 0.055f;
        //setup time for speed update
        nextSpeedUpdate = Time.time + speedUpdateTime;
        speedUpdateTime = 0.75f;
        speedIncrease = 0.1f;

        //set the number of balls
        CalculateNumberOfBalls();

        //used to put balls into, so easier viewing in the heirarchy
        ballContainer = new GameObject();
        ballContainer.name = "ballContainer";

        //Setup start pos at bottom of play area and the x pos based on where the drag started
        pos = camera.ScreenToWorldPoint(start);
        xPos = pos.x;
        //adjust ball start height if floor blocks in play
        if (floorBlock.floorBlocksActive == true)
        {
            yPos = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (ballSprite.transform.localScale.y / 5.5f) + levelGenerator.blockScaleAdjustedY;
        }
        else
        {
            yPos = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (ballSprite.transform.localScale.y / 5.5f);
        }

        //initialise balls
        balls = new Ball[GameManager.manager.maxNumberOfBalls];
        for (int n = 0; n < GameManager.manager.maxNumberOfBalls; n++)
        {
            balls[n] = new Ball
            {
                //active = false,
                position = new Vector2(xPos, yPos),
                ballType = Ball.TypeOfBall.normal,
                ball = Instantiate(ballSprite)
            };
            //put into ball conatiner for neatness
            balls[n].ball.transform.SetParent(ballContainer.transform);

            //change ball size for bonus level
            if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
            {
                balls[n].ball.transform.localScale *= 2;
            }
            else
            {
                balls[n].ball.transform.localScale *= 1.5f;
            }

            //set ball position
            balls[n].ball.transform.localPosition = balls[n].position;

        }
    }


    public IEnumerator MoveBalls(Vector3 start, Vector3 end)
    {
        Vector3 startPos, endPos;

        camera = FindObjectOfType<Camera>();

        if (GameManager.manager.ballsActive == true)
        {
            startPos = camera.ScreenToWorldPoint(start);
            endPos = camera.ScreenToWorldPoint(end);
            
            startPos.y = (-GameManager.manager.camY / 2) + (GameManager.manager.camY * GameManager.manager.freeBottomArea) + (ballSprite.transform.localScale.y / 5);

            Vector3 vel = (endPos - startPos).normalized * speed; //(end - start).normalized * speed;

            //Start all the balls moving
            for (int n = 0; n < GameManager.manager.maxNumberOfBalls; n++)
            {
                if((balls[n].ball != null) && GameManager.manager.ballsActive)
                {
                    Rigidbody2D rb = balls[n].ball.GetComponent<Rigidbody2D>();
                    rb.velocity = vel;
                    rb.rotation = 0;
                    yield return new WaitForSeconds(timeBetweenBalls);
                }

            }
        }
    }

    private void Update()
    {
        //Speed up balls over time
        if (Time.time > nextSpeedUpdate)
        {
            nextSpeedUpdate = Time.time + speedUpdateTime;
            //speed += speedIncrease;

            Time.timeScale += speedIncrease;
        }

        // Get all the balls and adjust colour as needed
        GameObject[] ball = GameObject.FindGameObjectsWithTag("ball");
        foreach(GameObject b in ball)
        {
            if((b != null) && (invincibleBalls.invincibleBallsActive==true))
            {
                b.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if(b!=null)
            {
                if(b.GetComponent<Rigidbody2D>().velocity.y >= 0)
                {
                    b.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    b.GetComponent<SpriteRenderer>().color = Color.grey;
                }
            }
        }
    }

    public IEnumerator DestroyBalls()
    {
        yield return new WaitForSeconds(1);
        for (int b = 0; b < GameManager.manager.maxNumberOfBalls; b++)
        {
            if (balls[b].ball != null)
            {
                Destroy(balls[b].ball);
            }
        }
    }
}

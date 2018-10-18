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

    public void InitializeBalls(Vector3 start)
    {
        Vector3 pos;

        camera = FindObjectOfType<Camera>();

        speed = 8;
        maxSpeed = 50; //38
        timeBetweenBalls = 0.040f;
        //setup time for speed update
        nextSpeedUpdate = Time.time + speedUpdateTime;
        speedUpdateTime = 0.5f;
        speedIncrease = 1f;

        //set the number of balls
        //Double balls for bonus level
        if (GameManager.manager.currentLevel % GameManager.manager.bonusLevel == 0)
            GameManager.manager.maxNumberOfBalls = 150;

        GameManager.manager.currentNumberOfBalls = GameManager.manager.maxNumberOfBalls;

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

            //Set colour for invincible balls
            if (invincibleBalls.invincibleBallsActive == true)
            {
                SpriteRenderer ball = balls[n].ball.GetComponent<SpriteRenderer>();
                ball.color = Color.red;
            }
        }


    }


    public IEnumerator MoveBalls(Vector3 start, Vector3 end)
    {
        Vector3 startPos, endPos;
        float minYVel = 0.038f;

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
        
        
        //Speed checks
        while(GameManager.manager.ballsActive)
        {
            //Speed up balls over time
            if (Time.time > nextSpeedUpdate)
            {
                nextSpeedUpdate = Time.time + speedUpdateTime;
                speed += speedIncrease;

                //increase speed
                if (speed <= maxSpeed)
                {
                    //help to prevent ball from moving through objects (does stop them though)
                    yield return new WaitForFixedUpdate();

                    for (int n = 0; n < GameManager.manager.maxNumberOfBalls; n++)
                    {
                        if (balls[n].ball != null)
                        {
                            Rigidbody2D rb = balls[n].ball.GetComponent<Rigidbody2D>();
                            Vector3 vel = rb.velocity.normalized * speed;

                            rb.velocity = vel;
                        }
                    }
                }
            }

            yield return null;

             /*
            //attempt to stop hoizontal movement
            for (int n = 0; n < GameManager.manager.maxNumberOfBalls; n++)
            {
                if (balls[n].ball != null)
                {
                    Rigidbody2D rb = balls[n].ball.GetComponent<Rigidbody2D>();
                    Vector2 vel = rb.velocity / speed;

                    if ((vel.y < minYVel) && (vel.y > -minYVel))
                    {
                        print("YPos!!");

                        if (vel.y > 0)
                        {
                            rb.AddForce(new Vector2(-1, vel.y+1)*1);

                            //vel.y += minYVel;
                        }
                        else
                        {
                            //vel.y -= minYVel;
                            rb.AddForce(new Vector2(1, vel.y-1) * 1);
                        }

                        //rb.velocity = vel;
                    }

                }
                
                
            }
            */
            yield return null;
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

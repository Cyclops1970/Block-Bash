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

    public void InitializeBalls(Vector3 start)
    {
        Vector3 pos;

        camera = FindObjectOfType<Camera>();

        speed = 8;
        maxSpeed = 50; //38
        timeBetweenBalls = 0.040f;
        //setup time for speed update
        nextSpeedUpdate = Time.time + speedUpdateTime;
        speedUpdateTime = 0.75f;
        speedIncrease = .5f;

        GameManager.manager.currentNumberOfBalls = GameManager.manager.maxNumberOfBalls;

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
                active = false,
                position = new Vector2(xPos, yPos),
                ballType = Ball.TypeOfBall.normal,
                ball = Instantiate(ballSprite)
            };
            //change ball size for bonus level
            if (GameManager.manager.currentLevel % 10 == 0)
            {
                balls[n].ball.transform.localScale *= 2;
            }

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
        float minXVel = 0.5f;
        float minYVel = 0.5f;
        float velOffset = 1;

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
        
        
        //Speed and angular checks
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
                            /*   
                            //attempt to stop vertical or hoizontal movement
                            if((rb.velocity.y < minYVel) && (rb.velocity.y > -minYVel))
                            {
                                vel = new Vector2(rb.velocity.x, velOffset).normalized;

                                rb.velocity = vel;
                            }
                            if ((rb.velocity.x < minXVel) && (rb.velocity.x > -minXVel))
                            {
                                vel = new Vector2(velOffset, rb.velocity.y).normalized;
                                rb.velocity = vel;
                            }
                            //yield return null;
                            */
                        }
                    }
                }
            }
            
            yield return null;
        }
        
    }

    /*
     * void BallBounce()
     * {
     *     void OnCollisionEnter( Collision col ) {
        Vector3 temp = Vector3.Cross( col.contacts[0].normal, rb.velocity );
        Vector3 tangent  = Vector3.Cross( col.contacts[0].normal, temp );
 
        Vector3 tangent_component = Vector3.Project( rb.velocity, tangent );
        Vector3 normal_component = Vector3.Project( rb.velocity, col.contacts[0].normal );
 
        rb.velocity = tangent_component + normal_component;
    }
    */


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

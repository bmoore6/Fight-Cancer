using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    //exploding pig particle effect
    [SerializeField]
    GameObject pigParticle;
    //timer fields
    Timer spawnTimer;
    [SerializeField]
    float timeDelay;

    //assign sprites
    [SerializeField]
    Sprite FrontSprite;

    [SerializeField]
    Sprite BackSprite;

    [SerializeField]
    Sprite LeftSprite;

    [SerializeField]
    Transform[] patrolPoints;

    Rigidbody2D rb2d;

    float speed = 40f;

    Transform currentPatrolPoint;

    int currentPatrolIndex;

    SpriteRenderer sr;

    //count how many times the pig gets shot
    private int hitCounter = 0;
    private int hitsBeforeDeath = 5;

    // Use this for initialization
    void Start()
    {
        //timer support
        // create and start timer
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = timeDelay;
        spawnTimer.Run();

        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
        rb2d.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer.Finished == true)
        {
            // get direction to move in
            float deltaX = currentPatrolPoint.position.x - transform.position.x;
            float deltaY = currentPatrolPoint.position.y - transform.position.y;
            float rads = Mathf.Atan2(deltaY, deltaX);
            float angle = Mathf.Rad2Deg * rads;
            float x = Mathf.Cos(rads);
            float y = Mathf.Sin(rads);

            //moving down left
            if (x <= 0 && y <= 0)
            {
                sr.flipX = false;
                sr.sprite = FrontSprite;
            }
            //moving down right
            else if (x >= 0 && y <= 0)
            {
                sr.flipX = true;
                sr.sprite = FrontSprite;
            }
            //moving up left
            else if (x <= 0 && y >= 0)
            {
                sr.flipX = false;
                sr.sprite = BackSprite;
            }
            //moving up right
            else if (x >= 0 && y >= 0)
            {
                sr.flipX = true;
                sr.sprite = BackSprite;
            }


            // move in that direction
            //transform.Translate(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Time.deltaTime * speed);
            rb2d.MovePosition(transform.position + new Vector3(x, y, 0) * speed * Time.deltaTime);

            //check if arrived at patrol point 
            if (Vector3.Distance(transform.position, currentPatrolPoint.position) < .4f)
            {
                //we have reached patrol point
                //check to see if there are more patrol points
                //if not return to first patrol point
                if (currentPatrolIndex + 1 < patrolPoints.Length)
                {
                    currentPatrolIndex++;

                }
                else
                {
                    currentPatrolIndex = 0;
                }
                currentPatrolPoint = patrolPoints[currentPatrolIndex];
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            hitCounter += 1;
            if (hitCounter >= hitsBeforeDeath)
            {
                //destroy the pig!
                Instantiate(pigParticle, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameManager.win();
            }

            //destroy the bullet that collided with the pig
            Destroy(collision.gameObject);
        }
    }
}
	


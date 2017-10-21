using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];
	}
	
	// Update is called once per frame
	void Update () {
        // get direction to move in
        float deltaX = currentPatrolPoint.position.x - transform.position.x;
        float deltaY = currentPatrolPoint.position.y - transform.position.y;
        float rads = Mathf.Atan2(deltaY, deltaX);
        float angle = Mathf.Rad2Deg * rads;
        float x = Mathf.Cos(rads);
        float y = Mathf.Sin(rads);

        //moving down left
        if(x<=0 && y<=0)
        {
            sr.flipX = false;
            sr.sprite = FrontSprite;
        }
        //moving down right
        else if ( x >=0 && y <= 0)
        {
            sr.flipX = true;
            sr.sprite = FrontSprite;
        }
        //moving up left
        else if ( x <=0 && y>=0)
        {
            sr.flipX = false;
            sr.sprite = BackSprite;
        }
        //moving up right
        else if(x>=0 && y>=0)
        {
            sr.flipX = true;
            sr.sprite = BackSprite;
        }


        // move in that direction
        //transform.Translate(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Time.deltaTime * speed);
        rb2d.MovePosition(transform.position + new Vector3(x, y, 0)  * speed * Time.deltaTime);

        //check if arrived at patrol point 
        if (Vector3.Distance(transform.position, currentPatrolPoint.position) < .4f)
        {
            //we have reached patrol point
            //check to see if there are more patrol points
            //if not return to first patrol point
            if(currentPatrolIndex + 1 < patrolPoints.Length)
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

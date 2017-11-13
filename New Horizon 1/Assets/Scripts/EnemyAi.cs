using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    //exploding pig particle effect
    [SerializeField]
    GameObject pigParticle;

    //assign sprites
    [SerializeField]
    Sprite FrontSprite;

    [SerializeField]
    Sprite BackSprite;

    [SerializeField]
    Sprite LeftSprite;

    // flock of pigs that the big pig can spawn
    [SerializeField]
    GameObject flockOpigs;

    Rigidbody2D rb2d;

    float speed = 40f;

    SpriteRenderer sr;

    //count how many times the pig gets shot
    private int hitCounter = 0;
    private int hitsBeforeDeath = 25;

    //pig's current tree target
    GameObject treeTarget;

    //object state
    public enum State {wandering, attackTree, still};
    public State pigState;

    //navigation
    Vector2 prevPosition;//pig's position during the previous frame
    Vector3 direction;//pig's random direction
    int moveAwayTime;

    // Use this for initialization
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb2d.freezeRotation = true;
        pigState = State.wandering;
        direction = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, 0).normalized;
        moveAwayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // get direction to move in
        float deltaX = prevPosition.x - transform.position.x;
        float deltaY = prevPosition.y - transform.position.y;
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
        if (pigState == State.still)
        {
            direction = (treeTarget.transform.position - gameObject.transform.position).normalized;
        }
      
        // move in that direction if not still
        //transform.Translate(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Time.deltaTime * speed);
        rb2d.MovePosition(transform.position + direction * speed * Time.deltaTime);


        //testing merge
        //set previous position to current
        if(pigState!=State.still)
        prevPosition = gameObject.transform.position;
    }
    private void FixedUpdate()
    {
        if (moveAwayTime > 0)
        {
            moveAwayTime--;
        }
        if (moveAwayTime == 1)
        {
            direction = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, 0).normalized;
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
                Instantiate(flockOpigs, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameManager.win();
            }

            //destroy the bullet that collided with the pig
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag != "tree"||collision.gameObject.GetComponent<TreeScript>().Health<.05f)
        {
            direction = direction * -1;
            moveAwayTime = 3;
            gameObject.transform.position = prevPosition;
        }
        else
        {
            pigState = State.still;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "tree" && collision.gameObject.GetComponent<TreeScript>().Health <= 0)
        {
            MoveOn();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tree"&&collision.gameObject.GetComponent<TreeScript>().Health > .05f)
        {
            treeTarget = collision.gameObject;
            pigState = State.attackTree;
            direction = (treeTarget.transform.position- gameObject.transform.position).normalized;
        }
    }
    public void MoveOn()
    {
        gameObject.transform.position = prevPosition;
        pigState = State.wandering;
        direction = direction * -1;
        moveAwayTime = 3;
    }
}
	


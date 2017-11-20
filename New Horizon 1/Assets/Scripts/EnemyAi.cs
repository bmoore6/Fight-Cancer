using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private enum Facing { down, up };

    //exploding pig particle effect
    [SerializeField]
    GameObject pigParticle;

    //assign sprites
    [SerializeField]
    GameObject frontSprites;

    [SerializeField]
    GameObject backSprites;

    // flock of pigs that the big pig can spawn
    [SerializeField]
    GameObject flockOpigs;

    Rigidbody2D rb2d;

    float speed = 40f;

    //count how many times the pig gets shot
    int hitCounter = 0;
    int hitsBeforeDeath = 25;

    private Animator anim;
    private Facing directionFacing;

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
        rb2d.freezeRotation = true;
        pigState = State.wandering;
        direction = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, 0).normalized;
        if(direction.y <= 0)
        {
            directionFacing = Facing.down;
        }
        else
        {
            directionFacing = Facing.up;
        }
        moveAwayTime = 0;
        anim = GetComponent<Animator>();
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
        if (x < 0 && y < 0)
        {
            transform.localScale = new Vector3(3f, 3f, 1f);
            directionFacing = Facing.up;
        }
        //moving down right
        else if (x > 0 && y < 0)
        {
            transform.localScale = new Vector3(-3f, 3f, 1f);
            directionFacing = Facing.up;
        }
        //moving up left
        else if (x < 0 && y > 0)
        {
            transform.localScale = new Vector3(-3f, 3f, 1f);
            directionFacing = Facing.down;
        }
        //moving up right
        else if (x > 0 && y > 0)
        {
            transform.localScale = new Vector3(3f, 3f, 1f);
            directionFacing = Facing.down;
        }

        if(directionFacing == Facing.down)
        {
            frontSprites.SetActive(true);
            backSprites.SetActive(false);
        }
        else
        {
            frontSprites.SetActive(false);
            backSprites.SetActive(true);
        }

        if (pigState == State.still)
        {
            rb2d.MovePosition(transform.position + direction * speed/10 * Time.deltaTime);
            if (direction.y < 0)
            {
                directionFacing = Facing.up;
            }
            else
            {
                directionFacing = Facing.down;
            }
            if(directionFacing == Facing.down)
            {
                anim.Play("idle_front");
            }
            else
            {
                anim.Play("idle_back");
            }
        }

        //testing merge
        //set previous position to current
        if (pigState != State.still)
        {
            prevPosition = gameObject.transform.position;
            if(directionFacing == Facing.down)
            {
                anim.Play("walk_front");
            }
            else
            {
                anim.Play("walk_back");
            }
            // move in that direction if not still
            //transform.Translate(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Time.deltaTime * speed);
            rb2d.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
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

    /// <summary>
    /// Damage the pig
    /// </summary>
    public void AddDamage()
    {
        hitCounter += 1;
        if (hitCounter >= hitsBeforeDeath)
        {
            //destroy the pig!
            Instantiate(pigParticle, gameObject.transform.position, Quaternion.identity);
            Instantiate(flockOpigs, gameObject.transform.position, Quaternion.identity);
            GameManager.GM.win();
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {       
        if (collision.gameObject.tag != "tree"||collision.gameObject.GetComponent<TreeScript>().Health<.15f)
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
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "tree")
        {
            MoveOn();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "tree" && collision.gameObject.GetComponent<TreeScript>().Health <= .15)
        {
            MoveOn();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tree"&&collision.gameObject.GetComponent<TreeScript>().Health > .15f&&pigState==State.wandering)
        {
            treeTarget = collision.gameObject;
            pigState = State.attackTree;
            direction = (treeTarget.transform.position- gameObject.transform.position).normalized;
        }
    }
    public void MoveOn()
    {
        treeTarget = null;
        gameObject.transform.position = prevPosition;
        pigState = State.wandering;
        direction = direction * -1;
        moveAwayTime = 2;
    }
}
	


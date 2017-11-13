using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private enum Facing { down, up };

    [SerializeField]
    GameObject BulletPrefab;

    [SerializeField]
    GameObject frontSprites;

    [SerializeField]
    GameObject backSprites;

    //set move speed
    public float moveSpeed = 3f;

    //scale
    Vector3 Scale;

    //get rigid body
    Rigidbody2D rb2d;

    //get sprite renderer
    SpriteRenderer sr;

    private Facing direction;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        direction = Facing.down;
        frontSprites.SetActive(true);
        frontSprites.transform.localScale = new Vector3(1f, 1f, 1f);
        backSprites.transform.localScale = new Vector3(-1f, 1f, 1f);
        backSprites.SetActive(false);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks for vertical axis input
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                //assign sprite front
                frontSprites.SetActive(true);
                backSprites.SetActive(false);
                direction = Facing.down;
            }
            else
            {
                //assigns sprite back
                frontSprites.SetActive(false);
                backSprites.SetActive(true);
                direction = Facing.up;
            }

            //Moves Forward and back along y axis  
            rb2d.MovePosition(transform.position + Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
        }

        // checks for horizontal axis input
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                frontSprites.transform.localScale = new Vector3(1f, 1f, 1f);
                backSprites.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                frontSprites.transform.localScale = new Vector3(-1f, 1f, 1f);
                backSprites.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            //Moves Left and right along x Axis  
            rb2d.MovePosition(transform.position + Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime);
        }

        rb2d.MovePosition(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * moveSpeed * Time.deltaTime);

        //calculate angle to shoot bullet at
        if (GameManager.GM.isPause() == false)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float deltaX = mousePosition.x - transform.position.x;
            float deltaY = mousePosition.y - transform.position.y;
            float rads = Mathf.Atan2(deltaY, deltaX);
            float angle = Mathf.Rad2Deg * rads;
            //transform.eulerAngles = new Vector3(0, 0, angle);
            //get shoot input
            if (Input.GetMouseButton(0))
            {
                GameObject projectile = Instantiate(BulletPrefab);
                projectile.transform.position = transform.position;
                projectile.transform.eulerAngles = new Vector3(0, 0, angle);
                if (direction == Facing.down)
                {
                    anim.Play("shoot_front");
                }
                else
                {
                    anim.Play("shoot_back");
                }
            }
        }

        // Animation Logic

        if(Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
        {
            if(direction == Facing.down)
            {
                anim.Play("idle_front");
            }
            else
            {
                anim.Play("idle_back");
            }
        }

        if(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if(direction == Facing.down)
            {
                anim.Play("walk_front");
            }
            else
            {
                anim.Play("walk_back");
            }
        }

    }

    // check for collision with wall
    void OnCollisionEnter2D(Collision2D coll)
    {

        // if colliding with wall and moving vertically move back
        if (coll.gameObject.tag == ("Wall"))
        {
            rb2d.velocity = Vector3.zero;
        }
    }
}

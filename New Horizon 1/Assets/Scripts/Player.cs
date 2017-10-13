using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //assign sprites
    [SerializeField]
    Sprite FrontSprite;

    [SerializeField]
    Sprite BackSprite;

    [SerializeField]
    Sprite LeftSprite;

    [SerializeField]
    GameObject BulletPrefab;

    //set move speed
    public float moveSpeed = 3f;

    //scale
    Vector3 Scale;

    //get rigid body
    Rigidbody2D rb2d;

    //get sprite renderer
    SpriteRenderer sr;



    // Use this for initialization
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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
                sr.sprite = FrontSprite;
            }
            else
            {
                //assigns sprite back
                sr.sprite = BackSprite;
            }

            //Moves Forward and back along y axis  
            rb2d.MovePosition(transform.position + Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
        }

        // checks for horizontal axis input
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                sr.sprite = LeftSprite;
                sr.flipX = false;
            }
            else
            {
                sr.sprite = LeftSprite;
                sr.flipX = true;             
            }
            //Moves Left and right along x Axis  
            rb2d.MovePosition(transform.position + Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime);
        }

        //calculate angle to shoot bullet at
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float deltaX = mousePosition.x - transform.position.x;
        float deltaY = mousePosition.y - transform.position.y;
        float rads = Mathf.Atan2(deltaY, deltaX);
        float angle = Mathf.Rad2Deg * rads;
        //transform.eulerAngles = new Vector3(0, 0, angle);

        //get shoot input
        if (Input.GetMouseButtonDown(0))
        {
            GameObject projectile = Instantiate(BulletPrefab);
            projectile.transform.position = transform.position;
            projectile.transform.eulerAngles = new Vector3(0, 0, angle);
        }



    }

    // check for collision with wall
    void OnCollisionEnter2D(Collision2D coll)
    {

        // if colliding with wall and moving vertically move back
        if (coll.gameObject.tag == "Wall")
        {
            rb2d.velocity = Vector3.zero;

        }


    }
}

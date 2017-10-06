using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    
        public float moveSpeed = 3f;

        int wallCollision = 1;
        Rigidbody2D rb2d;

        // Use this for initialization
        void Start()
        {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            
            //Moves Forward and back along y axis                           //Up/Down
            transform.Translate(wallCollision*( Vector3.up * Time.deltaTime * Input.GetAxis("Vertical") * moveSpeed));
            //Moves Left and right along x Axis                               //Left/Right
            transform.Translate(wallCollision*(Vector3.right * Time.deltaTime * Input.GetAxis("Horizontal") * moveSpeed));
        }

        // check for collision with wall
        void OnCollisionEnter2D(Collision2D coll)
        {

        // if colliding with wall and moving vertically move back
        if(coll.gameObject.tag == "Wall" )
        {
            rb2d.velocity = Vector3.zero;
        }

    }
    }



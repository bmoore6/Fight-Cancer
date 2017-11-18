using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private enum Facing { down, up };

    [SerializeField]
    GameObject wallSplash;

    [SerializeField]
    GameObject beamPrefab;

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

        //initialize character orientation
        SetCharacterFrontFacing();
        SetCharacterRightFacing();

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
                SetCharacterFrontFacing();
            }
            else
            {
                SetCharacterRearFacing();
            }

            //Moves Forward and back along y axis  
            rb2d.MovePosition(transform.position + Vector3.up * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime);
        }

        // checks for horizontal axis input
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                SetCharacterRightFacing();
            }
            else
            {
                SetCharacterLeftFacing();
            }
            //Moves Left and right along x Axis  
            rb2d.MovePosition(transform.position + Vector3.right * Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime);
        }

        rb2d.MovePosition(transform.position + new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * moveSpeed * Time.deltaTime);

        //check if player if holding left mouse button, if so then fire cytotoxin beam
        if (Input.GetMouseButton(0) && GameManager.GM.isPause() == false) { FireCytoBeam(); }

        // Animation Logic
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0)
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

    /// <summary>
    /// Get the origin of the CytoBeam. This will require some tweaking later
    /// </summary>
    /// <returns></returns>
    Vector2 CalculateBeamOrigin() {

        Vector2 currentPlayerPosition = transform.position;
        //currentPlayerPosition.y += 3;
        return currentPlayerPosition;
    }

    /// <summary>
    /// Handles player firing cytoToxin beam
    /// </summary>
    void FireCytoBeam()
    {
        // calculate current beam origin
        Vector2 beamOrigin = CalculateBeamOrigin();

        // current mouse cursor position. Typecasting to Vector2.
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // calculate direction from player to mouse cursor
        Vector2 beamDirection = mousePos - beamOrigin;

        // Use the magnitude of beamDirection to set max distance parameter of RayCast2D
        float maxDistance = beamDirection.magnitude;
        beamDirection.Normalize();

        // Raycast2D hit will give us information about other colliders 
        RaycastHit2D hit = Physics2D.Raycast(beamOrigin, beamDirection, maxDistance, LayerMask.GetMask("littlePig", "pig", "wall"));

        //// check for collisions
        if (hit.collider != null)
        {
            //kill little pigs
            if (hit.collider.tag == "littlePig") { hit.collider.gameObject.GetComponent<Unit>().AddDamage(); }

            //kill big pigs
            else if (hit.collider.tag == "Pig") { hit.collider.gameObject.GetComponent<EnemyAi>().AddDamage(); }

            //Cytotoxin beam hitting wall....create a splash
            else { Instantiate(wallSplash, hit.point, Quaternion.identity); }

            //set endpoint of the line equal to the point of impact
            mousePos = hit.point;
        }

        // render a cytotoxin beam
        GameObject cytoBeam = Instantiate(beamPrefab);
        Vector3[] beamPos = new Vector3[2];
        beamPos[0] = beamOrigin; //point at which the line begins
        beamPos[1] = mousePos; //point at while the line ends

        //face character in direction of beam
        FaceCharacterInBeamDirection(beamPos);

        // Set beam start and end points--->Render beam to screen
        cytoBeam.GetComponent<LineRenderer>().SetPositions(beamPos);

        //if (direction == Facing.down)
        //{
        //    anim.Play("shoot_front");
        //}
        //else
        //{
        //    anim.Play("shoot_back");
        //}       
    }
    /// <summary>
    /// This method forces the player character to face the direction of the beam
    /// </summary>
    /// <param name="beam"></param>
    void FaceCharacterInBeamDirection(Vector3[] beam)
    {
        if (beam[0].y >= beam[1].y)
        {
            SetCharacterFrontFacing();
        }
        else { SetCharacterRearFacing(); }

        if (beam[0].x >= beam[1].x)
        {
            SetCharacterRightFacing();
        }
        else { SetCharacterLeftFacing(); }
    }

    /// <summary>
    /// Note: Method names are self-explanatory
    /// </summary>
    void SetCharacterFrontFacing()
    {
        //assign sprite front
        frontSprites.SetActive(true);
        backSprites.SetActive(false);
        direction = Facing.down;
    }
    void SetCharacterRearFacing()
    {
        //assigns sprite back
        frontSprites.SetActive(false);
        backSprites.SetActive(true);
        direction = Facing.up;
    }
    void SetCharacterRightFacing()
    {
        frontSprites.transform.localScale = new Vector3(1f, 1f, 1f);
        backSprites.transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    void SetCharacterLeftFacing()
    {
        frontSprites.transform.localScale = new Vector3(-1f, 1f, 1f);
        backSprites.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    // unit manager...handles much of the flock's information
    [SerializeField]
    public GameObject manager;

    //exploding pig particle effect
    [SerializeField]
    GameObject pigParticle;

    // current location of this little pig
    public Vector2 location = Vector2.zero;

    // current velocity of this little pig
    public Vector2 velocity;

    // the position where the flock will gravitate towards, this will eventually become locations of trees
    Vector2 goalPos = Vector2.zero;

    // force that's being applied to the little pig
    Vector2 currentForce;

    //count how many times the little pig gets shot
    private int hitCounter = 0;
    private int hitsBeforeDeath = 2;


    // Use this for initialization
    void Start () {

        velocity = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
        location = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
    		
	}

    // works out the vector towards target location
    Vector2 seek(Vector2 target)
    {
        return (target - location);
    }

    // add force to little pig rigidbody (AddForce requires a 3D vector)
    void applyForce (Vector2 f)
    {
        Vector3 force = new Vector3(f.x, f.y, 0);
        if (force.magnitude > manager.GetComponent <AllUnits>().maxforce)
        {
            force = force.normalized;
            force *= manager.GetComponent<AllUnits>().maxforce;
        }
        this.GetComponent<Rigidbody2D>().AddForce(force);

        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > manager.GetComponent<AllUnits>().maxvelocity)
        {
            this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized;
            this.GetComponent<Rigidbody2D>().velocity *= manager.GetComponent<AllUnits>().maxvelocity;
        }
    }

    /// <summary>
    /// Calculates average heading of the flock
    /// </summary>
    /// <returns></returns>
    Vector2 align()
    {
        float neighbordist = manager.GetComponent<AllUnits>().neighborDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject) continue;

            float d = Vector2.Distance(location, other.GetComponent<Unit>().location);

            if (d < neighbordist)
            {
                sum += other.GetComponent<Unit>().velocity;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            Vector2 steer = sum - velocity; // average heading of the group minus this unit's velocity
            return steer;
        }

        return Vector2.zero;
    }

    /// <summary>
    /// Calculates average position of the flock
    /// </summary>
    /// <returns></returns>
    Vector2 cohesion()
    {
        float neighbordist = manager.GetComponent<AllUnits>().neighborDistance;
        Vector2 sum = Vector2.zero;
        int count = 0;
        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject) continue;

            float d = Vector2.Distance(location, other.GetComponent<Unit>().location);

            if (d < neighbordist)
            {
                sum += other.GetComponent<Unit>().location;
                count++;
            }
        }
        if (count > 0)
        {
            sum /= count;
            return seek(sum);
        }

        return Vector2.zero;
    }

    // calculating and controlling the rules for the movement of the little pig
    void flock()
    {
        location = this.transform.position;
        velocity = this.GetComponent<Rigidbody2D>().velocity;

        if (manager.GetComponent<AllUnits>().obedient && Random.Range (0,50) <= 1)
        {
            Vector2 ali = align(); // get the alignment vector
            Vector2 coh = cohesion(); 
            Vector2 gl;
            if (manager.GetComponent<AllUnits>().seekGoal)
            {
                gl = seek(goalPos);
                currentForce = gl + ali + coh;
            }
            else
                currentForce = ali + coh;
            currentForce = currentForce.normalized;
        }

        if (manager.GetComponent<AllUnits>().willful && Random.Range (0,50) <= 1) 
        {
            if (Random.Range(0, 50) < 1) // change direction
                currentForce = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
        }

        applyForce(currentForce);
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

    // Update is called once per frame
    void Update () {

        flock();
        goalPos = manager.transform.position;		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    // unit manager...handles much of the flock's information
    [SerializeField]
    public GameObject manager;

    // exploding pig particle effect
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

    // count how many times the little pig gets shot
    int hitCounter = 0;
    int hitsBeforeDeath = 2;

    // % of health that a tree must have in order to be attacked by little pigs
    float attractiveTreeHealth = .15f;

    // this controls how agressively the little pigs move towards a target
    float forceMultiplier = 800;

    // the amount of damage that the little pigs inflict on the trees
    float damageAmount = .00001f;

    // Use this for initialization
    void Start () {

        velocity = new Vector2(Random.Range(0.01f, 0.1f), Random.Range(0.01f, 0.1f));
        location = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

        // randomly select sorting layer. This will allow a little pig to either be in front of a tree, or behind a tree
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(3, 6);
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
        this.GetComponent<Rigidbody2D>().AddForce(force*forceMultiplier, ForceMode2D.Force);

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

    /// <summary>
    /// Adds damage to the little pig
    /// </summary>
    public void AddDamage()
    {
        hitCounter += 1;
        if (hitCounter >= hitsBeforeDeath)
        {
            //destroy the pig!
            Instantiate(pigParticle, gameObject.transform.position, Quaternion.identity);
            RemoveSelfFromArray(); //Remove this instance from the Units[] array
            Destroy(gameObject);
            GameManager.GM.win();
        }
    }
    /// <summary>
    /// This method removes this littlepig from the unit array
    /// </summary>
    void RemoveSelfFromArray()
    {
        int index = 0;
        foreach (GameObject other in manager.GetComponent<AllUnits>().units)
        {
            if (other == this.gameObject)
            {
                manager.GetComponent<AllUnits>().units.RemoveAt(index);
                // Debug.Log(manager.GetComponent<AllUnits>().units.Count);
                break;
            }
            index++;
        }
    }
    /// <summary>
    /// Call flocking method. This happens in FixedUpdate because flock() contains calls to the physics engine
    /// </summary>
    void FixedUpdate () {
            flock();
	}
    /// <summary>
    /// Update the target position for the unit....this will usually be trees
    /// </summary>
    private void Update()
    {
        goalPos = UpdateGoalPos();
        if (Vector2.Distance((Vector2) this.transform.position, UpdateGoalPos()) < 5) { DamageTrees(); }
    } 

    Vector2 UpdateGoalPos()
    {
        int index = 0;
        int indexOfNearestTree = -1; // initialize to -1 in order to be sure that loop cycled properly
        float distance = 1000f;

        foreach (GameObject tree in manager.GetComponent<AllUnits>().GetTrees)
        {
            float distanceFromTree = Vector2.Distance(transform.position, tree.transform.position);
            if (distanceFromTree < distance && tree.GetComponent<TreeScript>().Health > attractiveTreeHealth) // check for nearer distance and tree with health greater than 5%
            {
                distance = distanceFromTree;
                indexOfNearestTree = index;
            }
            index++;
        }

        if (indexOfNearestTree != -1)
        {
            return manager.GetComponent<AllUnits>().GetTrees[indexOfNearestTree].transform.position; // return the location of the nearest tree
        }
        else { return Vector2.zero; }
    }
    
    /// <summary>
    /// ***Redundant code, will need to rework this method to be more efficient****
    /// </summary>
    void DamageTrees()
    {
        int index = 0;
        int indexOfNearestTree = -1; // initialize to -1 in order to be sure that loop cycled properly
        float distance = 1000f;


        //iterate through tree array to find the tree that is currently being attacked
        foreach (GameObject tree in manager.GetComponent<AllUnits>().GetTrees)
        {
            float distanceFromTree = Vector2.Distance(transform.position, tree.transform.position);
            if (distanceFromTree < distance && tree.GetComponent<TreeScript>().Health > attractiveTreeHealth) // check for nearer distance and tree with health greater than 5%
            {
                distance = distanceFromTree;
                indexOfNearestTree = index;
            }
            index++;
        }
        if (indexOfNearestTree != -1)
        {
            GameObject treeToDamage = manager.GetComponent<AllUnits>().GetTrees[indexOfNearestTree];
            treeToDamage.GetComponent<TreeScript>().Health -= damageAmount;
        }
    }
}
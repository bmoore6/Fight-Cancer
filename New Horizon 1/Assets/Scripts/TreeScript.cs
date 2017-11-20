using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour { 

    [SerializeField]
    Sprite[] trees;

    [SerializeField]
    Material mat;

    [SerializeField]
    float damage=.0001f;

    [SerializeField]
    float regen = .0001f;

    // Health of each tree must be between 0 and 1
    float health = 1;

    Renderer rend;
    Vector4 permColor;
    int regenTimer;
    bool collActive=true;

    // Use this for initialization
    void Start () {

        // Randomly select which sprite to use        
        GetComponent<SpriteRenderer>().sprite = trees[Random.Range(0, trees.Length)];
        GetComponent<SpriteRenderer>().material = mat;

        // Allow Color property of shader to be changed 
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("2D/Texture Color Alpha");
        permColor = rend.material.GetColor("_Color");
    }

    private void Update()
    {
        if (Health > .15 && Health < 1)
        {
            Health += regen;
        }
        if (collActive==false)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
        if (Health<.15&&collActive==true)
        {
            collActive = false;
            regenTimer = 500;
        }
    }

    //timer
    private void FixedUpdate()
    {
        if (regenTimer > 0)
        {
            regenTimer--;
        }
        if (regenTimer == 1)
        {
            Health = .16f;
            collActive = true;
        }
    }

    /// <summary>
    /// Give the game access to each tree's health
    /// </summary>
    public float Health
    {
       get { return health; }
       set
        {
            health = value;
            if (health > 1) { health = 1; }
            if (health < 0) { health = 0; }          
            ControlShader();
        }     
    }

	/// <summary>
    /// This method controls how the appearance of the tree changes according to its health
    /// </summary>
    /// <param name="health"></param>
    private void ControlShader ()
    {
        Color tempColor;
        tempColor = permColor * health;
        rend.material.SetColor("_Color", tempColor);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pig"&&health>0)
        {
            this.Health -= damage;
        }
        else if (health < 1)
        {
            this.Health += regen;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 150;
    public GameObject bulletParticle;

    // Use this for initialization
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float rads = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 direction;
        direction.x = Mathf.Cos(rads);
        direction.y = Mathf.Sin(rads);
        rb.AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Instantiate(bulletParticle, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}

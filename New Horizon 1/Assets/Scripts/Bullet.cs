using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed = 100;

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

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}

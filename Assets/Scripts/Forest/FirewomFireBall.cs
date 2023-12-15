using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewomFireBall : MonoBehaviour
{
    Rigidbody2D rb2D;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (transform.position.magnitude > 100f)
        // {
        //     Destroy(gameObject);
        // }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force)
    {
        rb2D.AddForce(direction * force);
    }
}

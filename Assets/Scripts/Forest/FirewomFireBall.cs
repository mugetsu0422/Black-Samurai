using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewomFireBall : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] int atk = 1;

    Rigidbody2D rb2D;
    Animator animator;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Collide with map
        rb2D.velocity = new Vector2(0, 0);
        animator.SetTrigger("Explode");
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collide with player
        if (other.CompareTag("Player"))
        {
            // Change Health here
            other.GetComponent<CharacterScript>().changeHealth(-atk);
        }
        rb2D.velocity = new Vector2(0, 0);
        animator.SetTrigger("Explode");
        Destroy(gameObject, 1f);
    }

    public void Launch(Vector2 direction, float force)
    {
        animator.SetFloat("LookX", direction.x);
        rb2D.AddForce(direction * force);
    }
}

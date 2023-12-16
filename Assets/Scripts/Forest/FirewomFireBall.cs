using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewomFireBall : MonoBehaviour
{
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
        // if (transform.position.magnitude > 100f)
        // {
        //     Destroy(gameObject);
        // }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // Change Health here
        }
        animator.SetTrigger("Explode");
        StartCoroutine(DestroyAfterDelay(1f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force)
    {
        animator.SetFloat("LookX", direction.x);
        rb2D.AddForce(direction * force);
    }
}

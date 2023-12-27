using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloatedbaneBullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
     public int atk = 1;

    public float lifetime = 2f; // Thời gian tồn tại của viên đạn

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterDelay(lifetime));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float speed)
    {
        rb2d.velocity = direction.normalized * speed;
        FlipBullet();
    }

    void FlipBullet()
    {
        if (rb2d.velocity.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (rb2d.velocity.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collide with player
        if (other.CompareTag("Player"))
        {
            // Change Health here
            other.GetComponent<CharacterScript>().changeHealth(-atk);
            Destroy(gameObject);
        }
    }
}

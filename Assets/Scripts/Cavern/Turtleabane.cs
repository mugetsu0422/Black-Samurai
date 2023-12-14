using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleabaneController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    float horizontal;
    public float speed;
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Hit");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Attack1");
            Launch();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("Attack2");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Dead");
            StartCoroutine(DestroyAfterDelay(2f));
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = new Vector2(horizontal, 0) * speed;
        rb2d.velocity = velocity;

        Vector2 move = new Vector2(horizontal, 0);
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Speed", move.magnitude);
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void Launch()
    {
        float yOffset = 0.7f; 
        Vector2 spawnPosition = rb2d.position + new Vector2(lookDirection.x, 0 + yOffset);
        GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);;

        TurtleabaneBullet bullet = bulletObject.GetComponent<TurtleabaneBullet>();
        bullet.Launch(lookDirection, 50);
    }
}

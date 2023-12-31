using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerofDeathSpell : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public float lifetime = 0.1f;
    public int atk = 1;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyAfterDelay(lifetime));
        animator = GetComponent<Animator>();
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

     public void Launch()
    {
        animator.SetTrigger("Spell");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collide with player
        if (other.CompareTag("Player"))
        {
            // Change Health here
            other.GetComponent<CharacterScript>().changeHealth(-atk);
        }
    }
}

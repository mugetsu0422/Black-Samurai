using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack2 : MonoBehaviour
{
    [SerializeField] float projectileForce = 2000f;
    int attackDamage = (int)(KaguraBachiData.Atk * KaguraBachiData.AttackMultiplier.Special2);
    public int getATK => attackDamage;
    Rigidbody2D rb2D;
    Animator animator;
    initgame initgame;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initgame = GameObject.Find("InitGame").GetComponent<initgame>();
        if (initgame)
        {
            transform.localScale = 4 * initgame.player_scale;
        }
    }

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    public void SetDirection(float direction)
    {
        animator.SetFloat("LookX", direction);
    }

    public void Launch(float direction)
    {
        rb2D.AddForce(new Vector2(direction, 0) * projectileForce);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy(gameObject);
        if (other.CompareTag("Enemy"))
        {
            rb2D.velocity = new Vector2(0, 0);
            animator.SetTrigger("Explode");
            Destroy(gameObject, 0.133f);
        }
    }

    void InvokeDestroy()
    {
        Destroy(gameObject);
    }
}

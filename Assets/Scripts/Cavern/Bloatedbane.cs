using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloatedbane : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Transform characterTransform;
    private bool isChasing = false;
    private bool isPatrolling = false;
    private float patrolDirection = 1.0f;
    public float patrolSpeed;
    public float chaseSpeed;
    public float patrolRange;
    public float chaseRange;
    public float attackRange;
    public float attackCooldown;
    private float lastAttackTime;
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    public GameObject bulletPrefab;
    public int hp = 10;
    int currentHP;
    int atk = 1;
    private BoxCollider2D boxCollider;
    private bool isDead = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider = GetComponent<BoxCollider2D>();
        isPatrolling = true;
        currentHP = hp;
    }

    void Update()
    {
        if (isDead)
            return;

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        Vector2 colliderSize = boxCollider.size;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, colliderSize, 5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && currentState.IsName("Attack2"))
            {
                CharacterScript characterScript = collider.GetComponentInParent<CharacterScript>();
                characterScript.changeHealth(-atk);
            }
        }

        float distanceToCharacter = Vector2.Distance(transform.position, characterTransform.position);

        if (distanceToCharacter < chaseRange)
        {
            isChasing = true;
            isPatrolling = false;
        }
        else
        {
            isChasing = false;
            isPatrolling = true;
        }

        if (isChasing)
        {
            Vector2 direction = (characterTransform.position - transform.position).normalized;
            rb2d.velocity = direction;

            if (distanceToCharacter < attackRange)
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("Attack2");
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("Attack1");
                    Launch();
                    lastAttackTime = Time.time;
                }
            }
        }
        else if (isPatrolling)
        {
            rb2d.velocity = new Vector2(patrolSpeed * patrolDirection, rb2d.velocity.y);

            if ((patrolDirection > 0 && transform.position.x >= initialPosition.x + patrolRange) ||
                (patrolDirection < 0 && transform.position.x <= initialPosition.x - patrolRange))
            {
                patrolDirection *= -1;
            }
        }
    }

    Vector3 initialPosition;

    void FixedUpdate()
    {
        Vector2 move = rb2d.velocity;
        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Speed", move.magnitude);
    }

    void Awake()
    {
        initialPosition = transform.position;
    }

    void Launch()
    {
        float yOffset = 0.7f;
        Vector2 spawnPosition = rb2d.position + new Vector2(lookDirection.x, 0 + yOffset);
        GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity); ;

        BloatedbaneBullet bullet = bulletObject.GetComponent<BloatedbaneBullet>();
        bullet.Launch(lookDirection, 50);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            var player = other.GetComponentInParent<CharacterScript>();
            ChangeHealth(-(int)player.getATK);
            player.ChangeKi(KaguraBachiData.KiRegeneratePerHit);
        }
    }

    void ChangeHealth(int amount)
    {
        if (amount < 0 && currentHP > 0)
        {
            animator.SetTrigger("Hit");
            currentHP = Mathf.Clamp(currentHP + amount, 0, hp);

            if (currentHP <= 0)
            {
                isDead = true;
                StartCoroutine(Dead());
            }
        }
    }

    IEnumerator Dead()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject, 1.3f);
        animator.SetTrigger("Dead");
    }
}

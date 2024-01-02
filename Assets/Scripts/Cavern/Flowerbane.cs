using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flowerbane : MonoBehaviour
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
    public int hp = 10;
    int currentHP;
    int atk = 1;
    private BoxCollider2D boxCollider;
    private bool isDead = false;
    public float timeInvincible = 2f;
    bool isInvincible = false;
    float invincibleTimer;
    public int parasiteEssenceDrop = 100;

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

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        Vector2 colliderSize = boxCollider.size;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, colliderSize, 5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && (currentState.IsName("Attack1") || currentState.IsName("Attack2")))
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

            if (distanceToCharacter < attackRange)
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    int randomAttack = Random.Range(1, 3);
                    animator.SetTrigger("Attack" + randomAttack);
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                rb2d.velocity = direction * chaseSpeed;
            }
        }
        else if (isPatrolling)
        {
            rb2d.velocity = new Vector2(patrolSpeed * patrolDirection, 0);

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
        if (isDead)
            return;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            var player = other.GetComponentInParent<CharacterScript>();
            ChangeHealth(-(int)player.getATK);
            player.ChangeKi(KaguraBachiData.KiRegeneratePerHit);
        }
        else if (other.CompareTag("SwordProjectile"))
        {
            ChangeHealth(-other.GetComponent<SpecialAttack2>().getATK);
        }
    }

    void ChangeHealth(int amount)
    {
        if (isInvincible)
        {
            return;
        }
        if (amount < 0 && currentHP > 0)
        {
            animator.SetTrigger("Hit");
            currentHP = Mathf.Clamp(currentHP + amount, 0, hp);
            isInvincible = true;
            invincibleTimer = timeInvincible;
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
        Destroy(gameObject, 1f);
        animator.SetTrigger("Dead");
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        player.ChangeParasiteEssence(parasiteEssenceDrop);
    }
}

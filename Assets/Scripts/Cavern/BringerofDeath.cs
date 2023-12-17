using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerofDeath : MonoBehaviour
{
    private Rigidbody2D rb2d;
    float horizontal;
    public float speed;
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    public GameObject spellPrefab;
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
    

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartPatrol();
    }

    void StartPatrol()
    {
        isPatrolling = true;
    }

    void Update()
    {
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
                    animator.SetTrigger("Attack");
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("Cast");
                    StartCoroutine(Launch(distanceToCharacter));
                    lastAttackTime = Time.time;
                }
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

    // Coroutine to destroy the object after a delay
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    IEnumerator Launch(float xOffset)
    {
        yield return new WaitForSeconds(1f);
        xOffset = xOffset - 1.5f;
        float lowestYPosition = Mathf.Min(rb2d.position.y, transform.position.y);
        float yOffset = -6f;
        if (lookDirection.x < 0)
        {
            xOffset = -xOffset;
        }
        Vector2 spawnPosition = rb2d.position + new Vector2(lookDirection.x + xOffset, lowestYPosition + yOffset);
        GameObject spellObject = Instantiate(spellPrefab, spawnPosition, Quaternion.identity); ;

        BringerofDeathSpell spell = spellObject.GetComponent<BringerofDeathSpell>();
        spell.Launch();
    }
}

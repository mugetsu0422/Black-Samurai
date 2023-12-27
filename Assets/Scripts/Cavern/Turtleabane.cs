using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleabaneController : MonoBehaviour
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
                    ResetAllTriggers();
                    animator.SetTrigger("Attack2");
                    lastAttackTime = Time.time;
                }
            }
            else {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    ResetAllTriggers();
                    animator.SetTrigger("Attack1");
                    Launch();
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

    void Launch()
    {
        float yOffset = 0.7f; 
        Vector2 spawnPosition = rb2d.position + new Vector2(lookDirection.x, 0 + yOffset);
        GameObject bulletObject = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);;

        TurtleabaneBullet bullet = bulletObject.GetComponent<TurtleabaneBullet>();
        bullet.Launch(lookDirection, 50);
    }

    void ResetAllTriggers()
    {
        AnimatorControllerParameter[] parameters = animator.parameters;

        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
    }
}
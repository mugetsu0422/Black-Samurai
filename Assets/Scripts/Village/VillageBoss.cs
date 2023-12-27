using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VillageBoss : MonoBehaviour
{
    private Rigidbody2D rb2d;
    float horizontal;
    public float speed;
    public int health;
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
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
            Debug.Log(distanceToCharacter);
            if (distanceToCharacter < attackRange)
            {
                if (Time.time - lastAttackTime > attackCooldown)
                {
                    int randomAttack = UnityEngine.Random.Range(1, 3);
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
            rb2d.velocity = new Vector2(patrolSpeed * patrolDirection, rb2d.velocity.y + Physics2D.gravity.y*Time.deltaTime);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            ChangeHealth(-(int)other.GetComponentInParent<CharacterScript>().getATK);
        }
        else if (other.CompareTag("SwordProjectile"))
        {
            ChangeHealth(-other.GetComponent<SpecialAttack2>().getATK);
        }
    }

    public void ChangeHealth(int x){
        hurt();
        health = Math.Max(0,health-x);
        if (health < 1){
            Dead();
        }
    }

    private void Dead(){

    }

    private void hurt(){

    }
}

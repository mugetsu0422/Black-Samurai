using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VillageBoss : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public GameObject bossboder;
    float horizontal;
    public float speed;
    public int health;
    Animator animator;
    private Vector2 lookDirection = new Vector2(1, 0);
    private Transform characterTransform;
    private bool isChasing = false;
    private float patrolDirection = 1.0f;
    public float patrolSpeed;
    public float chaseSpeed;
    public float patrolRange;
    public float chaseRange;
    public float attackRange;
    public float attackCooldown;
    public int parasiteEssenceDrop;
    private float lastAttackTime;
    private BoxCollider2D boxCollider;
    int atk = 1;
    public int MaxHealth = 50;
    bool dead = false;
    
    void Start()
    {
        health = MaxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        characterTransform = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider =  GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        Vector2 colliderSize = boxCollider.size;
        Collider2D collider = Physics2D.OverlapBox(rb2d.position, boxCollider.bounds.size, 0f, LayerMask.GetMask("Player"));
       if (collider && collider.CompareTag("Player"))
            {
                CharacterScript characterScript = collider.GetComponentInParent<CharacterScript>();
                characterScript.changeHealth(-atk);
            }
        var player = GameObject.FindWithTag("Player").GetComponent<CharacterScript>();
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 playerPosition = playerTransform.position;
        float distanceToCharacter = Vector2.Distance(transform.position, playerPosition);
        if (distanceToCharacter < chaseRange)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Vector2 direction = (playerPosition - transform.position).normalized;
            rb2d.velocity = direction;

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
        health = Math.Max(0,health+x);
        BossHealthbar.instance.SetValue(health / (float)MaxHealth);
        if (health < 1){
            Dead();
        }
    }

    private void Dead(){
        if (dead)
            return;
        animator.SetTrigger("Dead");
        Destroy(gameObject,10f);
        Destroy(bossboder);
        this.enabled = false;
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        player.ChangeParasiteEssence(parasiteEssenceDrop);
        KaguraBachiData.PureParasiteHeart += 1;
        PureHeartEssenceNotification.instance.openNotification();
        BossHealthbar.instance.SetEnable(false);
        BackgroundMusic.instance.victoriousBGM();
        StartCoroutine(offVictoryMusic());
        dead = true;
    }

    IEnumerator offVictoryMusic()
    {
        yield return new WaitForSeconds(6f);
        BackgroundMusic.instance.originalBGM();
    }

    private void hurt(){
        animator.SetTrigger("Hit");
    }
}

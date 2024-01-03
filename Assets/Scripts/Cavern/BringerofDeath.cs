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
    public int hp = 10;
    int currentHP;
    int atk = 1;
    private BoxCollider2D boxCollider;
    private bool isDead = false;
    public float timeInvincible = 2f;
    bool isInvincible = false;
    float invincibleTimer;
    bool healthbar = false;
    public int parasiteEssenceDrop = 100;
    public GameObject bossZone;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        isPatrolling = true;
        currentHP = hp;
        BossHealthbar.instance.SetBossName("Bringer of Deadth");
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, colliderSize, 20f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player") && currentState.IsName("Attack"))
            {
                CharacterScript characterScript = collider.GetComponentInParent<CharacterScript>();
                characterScript.changeHealth(-atk);
            }
        }
        var player = GameObject.FindWithTag("Player").GetComponent<CharacterScript>();
        Transform playerTransform = player.GetComponent<Transform>();
        Vector3 playerPosition = playerTransform.position;
        float distanceToCharacter = Vector2.Distance(transform.position, playerPosition);

        if (distanceToCharacter < chaseRange)
        {
            if (!healthbar)
            {
                healthbar = true;
                BossHealthbar.instance.SetEnable(true);
                bossZone.SetActive(true);
                BackgroundMusic.instance.changeBossBGM();
            }
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
            Vector2 direction = (playerPosition - transform.position).normalized;
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
            BossHealthbar.instance.SetValue(currentHP / (float)hp);
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
        // Destroy(gameObject, 1.3f);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }
        animator.SetTrigger("Dead");
        BossHealthbar.instance.SetEnable(false);
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        player.ChangeParasiteEssence(parasiteEssenceDrop);
        KaguraBachiData.PureParasiteHeart += 1;
        PureHeartEssenceNotification.instance.openNotification();
        bossZone.SetActive(false);
        BackgroundMusic.instance.victoriousBGM();
        StartCoroutine(offVictoryMusic());
    }

    IEnumerator offVictoryMusic()
    {
        yield return new WaitForSeconds(6f);
        BackgroundMusic.instance.originalBGM();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cthulu : MonoBehaviour
{
    readonly string[] attackTypes = { "1Attack", "2Attack" };

    [Header("Health parameters")]
    [SerializeField] int hp = 500;
    int currentHP;

    [Header("Attack parameters")]
    [SerializeField] int atk = 1;
    [SerializeField] float attackIntervalTime = 1.5f;
    [SerializeField] Vector2 attackSize = Vector2.one;
    [SerializeField] Vector2 attackOriginOffset = Vector2.zero;
    [SerializeField] LayerMask attackLayerMask;
    [SerializeField] bool showGizmos = true;
    float attackTimer;

    [Header("Movement parameters")]
    [SerializeField] float direction = -1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float chaseSpeed = 20f;

    Rigidbody2D rb2D;
    Animator animator;
    AIPlayerDetector playerDetector;
    AIMeleeAttackDetector attackDetector;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponentInParent<AIPlayerDetector>();
        attackDetector = GetComponent<AIMeleeAttackDetector>();
        currentHP = hp;

        animator.SetFloat("LookX", direction);

        if (attackDetector != null)
        {
            attackDetector.OnPlayerDetected.AddListener(AttackPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            rb2D.velocity = new Vector2(0, rb2D.position.y);
            attackTimer -= Time.deltaTime;
            animator.SetBool("Walk", false);
            return;
        }
        else
        {
            animator.SetBool("Walk", true);
        }
        if (playerDetector && playerDetector.PlayerDetected)
        {
            var directionToTarget = (Vector2)playerDetector.Target.transform.position - rb2D.position;
            rb2D.velocity = new Vector2(chaseSpeed * Mathf.Sign(directionToTarget.x), rb2D.velocity.y);
            animator.SetFloat("LookX", directionToTarget.x >= 0 ? 1f : -1f);
        }

    }

    private void AttackPlayer(GameObject player)
    {
        if (attackTimer > 0)
        {
            return;
        }
        // Perform the attack logic here.
        var attackRandom = attackTypes[Random.Range(0, 2)];
        animator.SetTrigger(attackRandom);
        attackTimer = attackIntervalTime;
    }

    // Call in animation event
    void DealDamage()
    {
        Collider2D player = Physics2D.OverlapBox(rb2D.position + attackOriginOffset, attackSize, 0, attackLayerMask);
        if (player)
        {
            player.GetComponent<CharacterScript>().changeHealth(-atk);
        }
    }

    void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = new Color(0, 1f, 1f, 50f / 255f);
            Gizmos.DrawCube((Vector2)transform.position + attackOriginOffset, attackSize);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            ChangeHealth(-(int)other.GetComponentInParent<CharacterScript>().getATK);
        }
    }

    void ChangeHealth(int amount)
    {
        if (amount < 0 && currentHP > 0)
        {
            animator.SetTrigger("Hurt");
            currentHP = Mathf.Clamp(currentHP + amount, 0, hp);

            if (currentHP <= 0) {
                Dead();
            }
        }
    }

    void Dead() {
        Destroy(gameObject, 1.5f);
        animator.SetTrigger("Death");
    }
}

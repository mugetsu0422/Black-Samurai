using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedHuman : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] int hp = 27;
    [SerializeField] int atk = 1;

    [Header("Movement parameters")]
    [SerializeField] float speed = 10f;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] float distance = 3f;
    [SerializeField] float chaseSpeed = 20f;

    Rigidbody2D rb2D;
    float direction = 1;
    Vector2 initialPosition;

    [Header("Attack parameters")]
    [SerializeField] float attackTime = 1f;
    [SerializeField] Vector2 attackSize = Vector2.one;
    [SerializeField] Vector2 attackOriginOffset = Vector2.zero;
    [SerializeField] LayerMask attackLayerMask;
    [SerializeField] bool showGizmos = true;
    float attackTimer;

    Animator animator;
    AIPlayerDetector playerDetector;
    AIMeleeAttackDetector attackDetector;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponent<AIPlayerDetector>();
        attackDetector = GetComponent<AIMeleeAttackDetector>();
        initialPosition = rb2D.position;
        animator.SetBool("Run", true);

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
            return;
        }
        if (playerDetector && playerDetector.PlayerDetected)
        {
            rb2D.velocity = new Vector2(chaseSpeed * direction, rb2D.position.y);
        }
        else
        {
            rb2D.velocity = new Vector2(speed * direction, rb2D.position.y);
            if (rb2D.position.x > initialPosition.x + distance)
            {
                direction = -1;
            }
            else if (rb2D.position.x < initialPosition.x - distance)
            {
                direction = 1;
            }
            animator.SetFloat("LookX", direction);
        }


    }

    private void FixedUpdate()
    {

    }


    private void AttackPlayer(GameObject player)
    {
        // Perform the attack logic here.
        animator.SetTrigger("Attack");
        attackTimer = attackTime;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedFlying : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] int hp = 20;

    [Header("Movement parameters")]
    [SerializeField] float direction = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] float chaseSpeed = 20f;

    [Header("Attack parameters")]
    [SerializeField] int atk = 1;
    [SerializeField] float attackTime = 1f;
    [SerializeField] Vector2 attackSize = Vector2.one;
    [SerializeField] Vector2 attackOriginOffset = Vector2.zero;
    [SerializeField] LayerMask attackLayerMask;
    [SerializeField] bool showGizmos = true;
    float attackTimer;


    Rigidbody2D rb2D;
    Vector2 initialPosition;
    bool isInitialPosition = true;

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
        initialPosition = transform.position;
        animator.SetFloat("LookX", direction);

        if (attackDetector != null)
        {
            attackDetector.OnPlayerDetected.AddListener(AttackPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (attackTimer > 0)
        {
            rb2D.velocity = Vector2.zero;
            attackTimer -= Time.deltaTime;
            return;
        }
        if (playerDetector && playerDetector.PlayerDetected)
        {
            if (isInitialPosition)
            {
                isInitialPosition = false;
            }
            rb2D.position = Vector2.MoveTowards(rb2D.position, playerDetector.Target.transform.position, Time.deltaTime * chaseSpeed);
            animator.SetFloat("LookX", playerDetector.DirectionToTarget.x);
        }
        else
        {
            rb2D.position = Vector2.MoveTowards(rb2D.position, initialPosition, Time.deltaTime * speed);
            animator.SetFloat("LookX", direction);
        }
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

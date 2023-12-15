using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedFlying : MonoBehaviour
{
    [SerializeField] float direction = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float pauseTime = 1f;

    [Header("Chase player parameters")]
    [SerializeField] float chaseSpeed = 20f;


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
        // For this example, let's assume the enemy inflicts 10 damage to the player.
        // player.GetComponent<PlayerHealth>()?.TakeDamage(10);
        animator.SetTrigger("Attack");
    }
}

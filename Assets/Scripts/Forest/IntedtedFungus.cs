using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedFungus : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] float distance = 3f;

    [Header("Chase player parameters")]
    [SerializeField] float chaseSpeed = 20f;


    Rigidbody2D rb2D;
    float direction = 1;
    Vector2 initialPosition;
    float pauseTimer;
    bool isPaused = false;

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
        pauseTimer = pauseTime;

        if (attackDetector != null)
        {
            attackDetector.OnPlayerDetected.AddListener(AttackPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetector && playerDetector.PlayerDetected)
        {
            // transform.position = Vector2.MoveTowards(transform.position, playerDetector.Target.transform.position, chaseSpeed * Time.deltaTime);
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
        // For this example, let's assume the enemy inflicts 10 damage to the player.
        // player.GetComponent<PlayerHealth>()?.TakeDamage(10);
        animator.SetTrigger("Attack");
    }
}

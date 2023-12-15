using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedFlying : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float pauseTime = 1f;
    [SerializeField] float distance = 3f;

    [Header("Chase player parameters")]
    [SerializeField] float chaseSpeed = 20f;


    Rigidbody2D rb2D;
    int direction = 1;
    Vector2 initialPosition;
    bool isInitialPosition = true;

    Animator animator;
    AIPlayerDetector playerDetector;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerDetector = GetComponentInParent<AIPlayerDetector>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetector && playerDetector.PlayerDetected)
        {
            if (isInitialPosition)
            {
                isInitialPosition = false;
            }
            rb2D.velocity = Time.deltaTime * chaseSpeed * playerDetector.DirectionToTarget;
            animator.SetFloat("LookX", playerDetector.DirectionToTarget.x);
        }
        else
        {
            if (isInitialPosition)
            {
                rb2D.velocity = new Vector2(Time.deltaTime * speed * direction, 0);
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
            else
            {
                Debug.Log(rb2D.position);
                Debug.Log(initialPosition);
                if (rb2D.position == initialPosition)
                {
                    isInitialPosition = true;
                    return;
                }
                Vector2 backDirection = (initialPosition - rb2D.position).normalized;
                rb2D.velocity = Time.deltaTime * speed * backDirection;
                // transform.position = Vector2.MoveTowards(transform.position, initialPosition, Time.deltaTime * speed);
                // rb2D.MovePosition(initialPosition * Time.deltaTime * speed);
                animator.SetFloat("LookX", backDirection.x);
            }

        }


    }

    private void FixedUpdate()
    {

    }
}

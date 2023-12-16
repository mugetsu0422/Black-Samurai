using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworm : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField] float direction = 1;

    Animator animator;
    AIRangedAttackDetector attackDetector;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce = 600f;
    [SerializeField] float attackAnimationDelay = 0.875f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackDetector = GetComponent<AIRangedAttackDetector>();
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

    }


    private void AttackPlayer(GameObject player)
    {

        animator.SetTrigger("Attack");
    }

    void Attack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position + new Vector2(6f * direction, 3.5f), Quaternion.identity);
        FirewomFireBall projectile = projectileObject.GetComponent<FirewomFireBall>();
        projectile.Launch(new Vector2(direction, 0), projectileForce);

    }
}
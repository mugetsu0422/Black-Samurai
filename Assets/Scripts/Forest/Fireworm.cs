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
        // Perform the attack logic here.
        // For this example, let's assume the enemy inflicts 10 damage to the player.
        // player.GetComponent<PlayerHealth>()?.TakeDamage(10);
        Attack();
    }

    void Attack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb2D.position + Vector2.up * 3f, Quaternion.identity);

        FirewomFireBall projectile = projectileObject.GetComponent<FirewomFireBall>();
        projectile.Launch(new Vector2(direction, 0), projectileForce);

        animator.SetTrigger("Attack");
    }
}

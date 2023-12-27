using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworm : MonoBehaviour
{
    [Header("Health parameters")]
    [SerializeField] int hp = 53;
    int currentHP;
    Rigidbody2D rb2D;
    [SerializeField] float direction = 1;

    [Header("Attack parameters")]
    [SerializeField] int atk = 1;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileForce = 600f;

    [SerializeField] int parasiteEssenceDrop = 50;

    Animator animator;
    AIRangedAttackDetector attackDetector;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackDetector = GetComponent<AIRangedAttackDetector>();
        animator.SetFloat("LookX", direction);
        currentHP = hp;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            var player = other.GetComponentInParent<CharacterScript>();
            ChangeHealth(-(int)player.getATK);
            player.ChangeKi(KaguraBachiData.KiRegeneratePerHit);
        }
    }

    void ChangeHealth(int amount)
    {
        if (amount < 0 && currentHP > 0)
        {
            animator.SetTrigger("Hurt");
            currentHP = Mathf.Clamp(currentHP + amount, 0, hp);

            if (currentHP <= 0)
            {
                Dead();
            }
        }
    }

    void Dead()
    {
        Destroy(gameObject, 1.5f);
        animator.SetTrigger("Death");
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        player.ChangeParasiteEssence(parasiteEssenceDrop);
    }
}

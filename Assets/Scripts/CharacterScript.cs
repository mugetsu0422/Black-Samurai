using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    float horizontal;
    Vector2 lookDirection = new Vector2(1,0);
    Animator animator;
    public int maxHealth = 3;
    int currentHealth;

    public float timeInvincible = 1.5f;
    bool isInvincible = false;
    float invincibleTimer;

    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible){
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer <0){
                isInvincible = false;
            }
        }

        horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, 0);
        if (Mathf.Abs(move.x) > 0.05f)
        {
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
            animator.SetFloat("Move", move.x);
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            animator.SetTrigger("Attack1");
        }
        if(Input.GetKeyDown(KeyCode.X)){
            animator.SetTrigger("Attack2");
        }
        if(Input.GetKeyDown(KeyCode.C)){
            animator.SetTrigger("Attack3");
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            animator.SetTrigger("Jump");
        }
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x += 2f*horizontal * Time.deltaTime;
        rb2d.MovePosition(position);
    }

    public int health{
        get {return currentHealth;}
        set {currentHealth = value;}
    }

    public void changeHealth(int amount){
        if(amount<0){
            animator.SetTrigger("Hurt");
            if(isInvincible){
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(1,0);
    Animator animator;
    public int maxHealth = 3;
    int currentHealth;

    public float timeInvincible = 1.5f;
    bool isInvincible = false;
    float invincibleTimer;

    [SerializeField] float speed = 100f;
    [SerializeField] float jumpForce = 500f;

    
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
        if(currentHealth == 0){
            Dead();
        }
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
            Attack1();
        }
        if(Input.GetKeyDown(KeyCode.X)){
            Attack2();
        }
        if(Input.GetKeyDown(KeyCode.C)){
            Attack3();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }
        
    }

    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x += speed *horizontal * Time.deltaTime;
        rb2d.MovePosition(position);
    }

    void Attack1(){
        animator.SetTrigger("Attack1");
    }

    void Attack2(){
        animator.SetTrigger("Attack2");
    }

    void Attack3(){
        animator.SetTrigger("Attack3");
    }

    void Jump(){
        animator.SetTrigger("Jump");
        rb2d.AddForce(Vector2.up * jumpForce);
    }

    void Dead(){
        animator.SetTrigger("Dead");
    }

    public int health{
        get {return currentHealth;}
        set {currentHealth = value;}
    }

    public void changeHealth(int amount){
        if(amount<0){
            animator.SetTrigger("Hurt");
            animator.Play("ChangeColour");
            if(isInvincible){
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}

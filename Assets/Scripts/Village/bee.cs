using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class bee : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector2 look_direction;
    private Vector2 direction;
    private Animator ani;
    public int health=1;
    public float speed;
    public GameObject projectile;
    public GameObject shoot_position;
    public float force;
    private bool can_shoot = true;
    private GameObject player;
    Vector3 initPosition;
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        ani.SetFloat("X",0.1f);
    }

    void Awake(){
        initPosition = gameObject.transform.position;
    }
    // Update is called once per frame
    void Update()
    {  
        look_direction = new Vector2( Convert.ToSingle(transform.GetComponent<SpriteRenderer>().flipX)-0.5f,0);
        look_direction.Normalize();

        RaycastHit2D hit = Physics2D.CircleCast(gameObject.transform.position,50,Vector2.zero,0,LayerMask.GetMask("Player"));
        if (hit.collider != null){
            RaycastHit2D obstacle = Physics2D.Linecast(gameObject.transform.position,hit.collider.transform.position,LayerMask.GetMask("Map"));
            if (obstacle.collider == null){
                Vector2 temp = hit.collider.transform.position - gameObject.transform.position;
                temp.Normalize();
                setMove(temp);
                if ( Mathf.Abs(temp.y)<0.1f){
                    Attack();
                    Invoke("Dead",3);
                }
            }
            else{
                Vector2 temp = initPosition - gameObject.transform.position;
                temp.Normalize();
                setMove(temp);
            }
        }
        else{
            Vector2 temp = initPosition - gameObject.transform.position;
            temp.Normalize();
            setMove(temp);
        }

        
        if (Mathf.Abs(direction.sqrMagnitude)>0.1){
            ani.SetFloat("X",direction.x);
            Vector2 positison = transform.position;
            positison.x += direction.x * speed * Time.deltaTime;
            positison.y += direction.y * speed * Time.deltaTime;
            transform.position = positison;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            ChangeHealth(-(int)other.GetComponentInParent<CharacterScript>().getATK);
        }
    }

    public void setMove(Vector2 _direction){
        direction = _direction;
    }

    public void Attack(){
        if (can_shoot){
            can_shoot = false;
            ani.SetTrigger("shoot");
            Invoke("_shoot",0.1f);
        }
    }
    private void _shoot(){
        GameObject temp = Instantiate(projectile,shoot_position.transform.position+ new Vector3(look_direction.x,look_direction.y,transform.position.z)*0.5f,gameObject.transform.rotation);
        temp.transform.localScale = gameObject.transform.localScale;
        temp.GetComponent<SpriteRenderer>().flipX = transform.GetComponent<SpriteRenderer>().flipX;
        temp.GetComponent<Rigidbody2D>().AddForce(look_direction * force);
    }

    public void hurt(){
        ani.SetTrigger("hurt");
    }

    public void Dead(){
        ani.SetTrigger("dead");
        Destroy(gameObject,1);
    }

    public void ChangeHealth(int x){
        hurt();
        health = Math.Max(0,health-x);
        if (health < 1){
            Dead();
        }
    }
}

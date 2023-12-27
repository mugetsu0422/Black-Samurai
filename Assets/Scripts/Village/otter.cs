using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Otter : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float force;
    public int health = 5;
    private Animator ani;
    private Rigidbody2D rgbd;
    private bool isjump = false;
    
    [Range(-1f, 1f)]
    public float horizontal = 0;
    
    void Awake()
    {
        ani = transform.GetComponent<Animator>();
        rgbd = transform.GetComponent<Rigidbody2D>();
        horizontal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(horizontal)>0.05){
            ani.SetFloat("X",horizontal);
            if (Mathf.Abs(horizontal)>0.2){
                ani.SetBool("Sleep",false);
                rgbd.velocity = new Vector2(horizontal*speed,rgbd.velocity.y);
            }
            else{
                rgbd.velocity = new Vector2(0,rgbd.velocity.y);
            }
        }
        if (isjump){
            Bounds boxbounds = gameObject.GetComponent<CapsuleCollider2D>().bounds;
            RaycastHit2D hit = Physics2D.Raycast(boxbounds.center,Vector2.down,boxbounds.extents.y+1.1f,LayerMask.GetMask("Map"));
            if (hit.collider!=null){
                    ani.SetTrigger("Land");
                    isjump = false;
            }
        }
        else{
            Bounds boxbounds = gameObject.GetComponent<CapsuleCollider2D>().bounds;
            RaycastHit2D hit = Physics2D.Raycast(boxbounds.center,Vector2.down,boxbounds.extents.y+1.1f,LayerMask.GetMask("Map"));
            if (hit.collider==null){
                isjump = true;
                ani.SetTrigger("Jump");
            }
            else{
                rgbd.velocity.Set(rgbd.velocity.x,0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            ChangeHealth(-(int)other.GetComponentInParent<CharacterScript>().getATK);
        }
        else if (other.CompareTag("SwordProjectile"))
        {
            ChangeHealth(-other.GetComponent<SpecialAttack2>().getATK);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player"){
            collision.transform.GetComponent<CharacterScript>().changeHealth(-1);
        }
    }

    public void ChangeHealth(int x){
        health = Math.Max(0,health-x);
        if (health < 1){
            Dead();
        }
    }

    public void setMove(float _horizontal){
        horizontal = _horizontal;
    }

    public void Jump(){
        if( isjump == false){
            isjump = true;
            Wake();
            rgbd.velocity =  Vector2.up*force;
            ani.SetTrigger("Jump");
        }
    }

    public void Sleep(){
        ani.SetBool("Sleep",true);
    }

    public void Wake(){
        ani.SetBool("Sleep",false);
    }
    void Dead(){
        horizontal = 0;
        ani.SetBool("Sleep",true);
        Destroy(gameObject,0);
    }
}

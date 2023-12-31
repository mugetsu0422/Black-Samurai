using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class wolf : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Mod stats")]
    [SerializeField] int parasiteEssenceDrop = 50;
    public float speed=20;
    public float force=2000;
    public int health=20;
    public int atk = 1;
    private Animator ani;
    private Rigidbody2D rgbd;
    private bool isjump = false;
    private float attackTime = -1;
    [Range(-1f, 1f)]
    private float horizontal = 0;

    private CapsuleCollider2D capCol;
    
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        capCol = gameObject.GetComponent<CapsuleCollider2D>();
        rgbd = gameObject.transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ///
        if (Mathf.Abs(horizontal)>0.05){
            ani.SetFloat("X",horizontal);
        }
        if (Mathf.Abs(horizontal)>=0.2){
            rgbd.velocity = new Vector2(horizontal*speed,rgbd.velocity.y);
        }
        else{
            rgbd.velocity = new Vector2(0,rgbd.velocity.y);
        }
        if (isjump){
            Bounds boxbounds = gameObject.GetComponent<BoxCollider2D>().bounds;
            RaycastHit2D hit = Physics2D.Raycast(boxbounds.center,Vector2.down,boxbounds.extents.y+0.5f,LayerMask.GetMask("Map"));
            if (hit.collider!=null){
                    isjump = false;
            }
        }
        else{
            Bounds boxbounds = gameObject.GetComponent<BoxCollider2D>().bounds;
            RaycastHit2D hit = Physics2D.Raycast(boxbounds.center,Vector2.down,boxbounds.extents.y+0.1f,LayerMask.GetMask("Map"));
            if (hit.collider==null){
                isjump = true;
            }
        }

        Collider2D player = Physics2D.OverlapBox(rgbd.position, capCol.bounds.size , 0, LayerMask.GetMask("Player"));
        if(player){
            player.GetComponent<CharacterScript>().changeHealth(-atk);
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
        if(Time.time - attackTime < 1 && collision.transform.tag == "Player"){
            Destroy(gameObject);
            collision.transform.GetComponent<CharacterScript>().changeHealth(-1);
        }
    }

    public void ChangeHealth(int x){
        ani.SetTrigger("Hit");
        health = Math.Max(0,health+x);
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
            rgbd.AddForce(Vector2.up*force);
        }
    }

    public void Attack(){
        attackTime=Time.time;
        ani.SetTrigger("Attack");
    }

    public void Dead(){
        ani.SetTrigger("Dead");
        this.enabled = false;
        Destroy(gameObject,1);
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterScript>();
        player.ChangeParasiteEssence(parasiteEssenceDrop);
    }
}

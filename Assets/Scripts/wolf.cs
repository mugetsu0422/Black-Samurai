using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolf : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed=20;
    public float force=2000;
    private Animator ani;
    private Rigidbody2D rgbd;
    private bool isjump = false;
    
    [Range(-1f, 1f)]
    public float horizontal = 0;
    
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        rgbd = gameObject.transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(horizontal)>0.05){
            ani.SetFloat("X",horizontal);
        }
        if (Mathf.Abs(horizontal)>=0.2){
            rgbd.velocity = new Vector2(horizontal*speed*100*Time.deltaTime,rgbd.velocity.y);
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
        ani.SetTrigger("Attack");
    }

    public void Dead(){
        ani.SetTrigger("Dead");
    }
}

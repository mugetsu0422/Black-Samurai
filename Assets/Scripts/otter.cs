using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Otter : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float force;
    private Animator ani;
    private Rigidbody2D rgbd;
    private bool isjump = false;
    private float default_speed;
    private Vector3 old_pos;
    
    [Range(-1f, 1f)]
    public float horizontal = 0;
    
    void Awake()
    {
        ani = transform.GetComponent<Animator>();
        rgbd = transform.GetComponent<Rigidbody2D>();
        default_speed = speed;
        horizontal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(horizontal)>0.05){
            ani.SetFloat("X",horizontal);
            if (Mathf.Abs(horizontal)>0.2){
                ani.SetBool("Sleep",false);
                rgbd.velocity = new Vector2(horizontal*speed*100*Time.deltaTime,rgbd.velocity.y);
            }
            else{
                rgbd.velocity = new Vector2(0,rgbd.velocity.y);
            }
        }
        if (isjump){
            Bounds boxbounds = gameObject.GetComponent<CapsuleCollider2D>().bounds;
            RaycastHit2D hit = Physics2D.Raycast(boxbounds.center,Vector2.down,boxbounds.extents.y+1.1f,LayerMask.GetMask("Map"));
            if (hit.collider!=null){
                    ani.ResetTrigger("Land");
                    ani.SetTrigger("Land");
                    isjump = false;
                    speed=default_speed;
            }
        }
        else{
            Bounds boxbounds = gameObject.GetComponent<CapsuleCollider2D>().bounds;
            RaycastHit2D hit = Physics2D.Raycast(boxbounds.center,Vector2.down,boxbounds.extents.y+1.1f,LayerMask.GetMask("Map"));
            if (hit.collider==null){
                isjump = true;
                ani.ResetTrigger("Jump");
                ani.SetTrigger("Jump");
            }
            else{
                rgbd.velocity.Set(rgbd.velocity.x,0);
            }
        }
    }

    public void setMove(float _horizontal){
        if (Mathf.Abs(_horizontal)<=0.05){
            horizontal = _horizontal < 0 ? -0.1f :0.1f;
        }
        else
            horizontal = _horizontal;
    }

    public void Jump(){
        if( isjump == false){
            isjump = true;
            Wake();
            rgbd.velocity =  Vector2.up*force;
            ani.SetTrigger("Jump");
            speed*=2.5f;
        }
    }

    public void Sleep(){
        horizontal = 0;
        ani.SetBool("Sleep",true);
    }

    public void Wake(){
        ani.SetBool("Sleep",false);
    }
}

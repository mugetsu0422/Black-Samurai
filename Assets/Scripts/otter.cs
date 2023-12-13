using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class otter : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float force;
    private Animator ani;
    private Rigidbody2D rgbd;
    private bool isjump = false;
    private float default_speed;
    
    [Range(-1f, 1f)]
    public float horizontal = 0;
    
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        rgbd = gameObject.transform.GetComponent<Rigidbody2D>();
        default_speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

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
                ani.SetTrigger("Jump");
            }
            else{
                rgbd.velocity.Set(rgbd.velocity.x,0);
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
            ani.SetTrigger("Jump");
            speed*=2.5f;
        }
    }

    public void Sleep(){
        ani.SetBool("Sleep",true);
    }

    public void Wake(){
        ani.SetBool("Sleep",false);
    }
}

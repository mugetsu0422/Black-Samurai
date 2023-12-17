using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bee : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector2 look_direction;
    private Vector2 direction;
    private Animator ani;
    public float speed;
    public GameObject projectile;
    public GameObject shoot_position;
    public float force;
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();
        ani.SetFloat("X",0.1f);
    }

    // Update is called once per frame
    void Update()
    {  
        look_direction = new Vector2( Convert.ToSingle(transform.GetComponent<SpriteRenderer>().flipX)-0.5f,0);
        look_direction.Normalize();

        if (Mathf.Abs(direction.sqrMagnitude)>0.1){
            ani.SetFloat("X",direction.x);
            Vector2 positison = transform.position;
            positison.x += direction.x * speed * Time.deltaTime;
            positison.y += direction.y * speed * Time.deltaTime;
            transform.position = positison;
        }
    }

    public void setMove(Vector2 _direction){
        direction = _direction;
    }

    public void Attack(){
        ani.SetTrigger("shoot");
        Invoke("_shoot",0.1f);
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
    }
}

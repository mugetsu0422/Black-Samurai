using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class mobspawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mob;
    public BoxCollider2D area;
    public int Max_mobs = 10;
    public float reproductive_rate;
    void Start()
    {
        Invoke("spawn",0);
    }

    void spawn(){
        Vector2 pos = gameObject.transform.position;
        pos += area.offset;
        RaycastHit2D[] hit = Physics2D.BoxCastAll( pos,area.size,0,Vector2.zero,0,LayerMask.GetMask("Enemy"));
        int n = hit.ToList().FindAll(e => e.transform.name == mob.transform.name).Count;
        if (n < Max_mobs){
            pos.x += UnityEngine.Random.Range(-area.size.x/2,area.size.x/2);
            pos.y += UnityEngine.Random.Range(-area.size.y/3,area.size.y/3);
            GameObject _mob = Instantiate(mob, pos, new Quaternion());
            _mob.transform.name = mob.transform.name;
        }
        
        Invoke("spawn",reproductive_rate);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Otter_controler : MonoBehaviour
{
    // Start is called before the first frame update
    public Otter otter;
    private GameObject player = null;

    private bool is_Sleep;

    private float detected_time;
    private float min_y,max_y;
    void Start()
    {
        is_Sleep = true;
        if (otter != null)
            otter.Sleep();
        min_y = otter.transform.position.y;
        max_y = min_y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        otter.Jump();
        /*
        if(is_Sleep){
            RaycastHit2D hit = Physics2D.BoxCast(gameObject.transform.position, new Vector2(50,5),0,Vector2.zero,0,LayerMask.GetMask("Player"));
            
            if (hit.collider!= null && hit.collider.tag == "Player"){
                otter.Wake();
                is_Sleep = false;
                detected_time = Time.time;
                player = hit.collider.gameObject;
            }
        }
        else{

        }
        */
    }
}

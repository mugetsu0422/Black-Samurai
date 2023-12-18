using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Otter_controller : MonoBehaviour
{
    // Start is called before the first frame update
    public Otter otter;
    private GameObject player = null;

    private bool is_Sleep;
    public Vector2 detect_size;
    public float Sensitivity;
    public float max_trace;
    public float time_trace;
    private float defaultSensitivity;
    private float _max_trace;
    private float detected_time;
    private float last_time_jump;
    private Vector3 old_pos;
    void Start()
    {
        is_Sleep = true;
        if (otter != null)
            otter.Sleep();
        _max_trace = max_trace*max_trace;
        defaultSensitivity = Sensitivity;
        last_time_jump = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(is_Sleep){
            RaycastHit2D hit = Physics2D.BoxCast(gameObject.transform.position, detect_size,0,Vector2.zero,0,LayerMask.GetMask("Player"));
            
            if (hit.collider!= null && hit.collider.tag == "Player"){
                otter.Wake();
                is_Sleep = false;
                detected_time = Time.time;
                player = hit.collider.gameObject;
                Sensitivity = 0;
                old_pos = transform.position;
            }
        }
        else{
            Vector2 direc = player.transform.position - gameObject.transform.position;
            Sensitivity -= Time.deltaTime;
            last_time_jump -=Time.deltaTime;
            if(MathF.Abs(direc.x) > Mathf.Abs(direc.y) && direc.sqrMagnitude < _max_trace && Sensitivity < 0 ){
                if( Mathf.Abs((transform.position - old_pos).x) <Time.deltaTime){
                    if (last_time_jump < 0){
                        otter.Jump();
                        last_time_jump = 3;
                    }                
                }
                detected_time = Time.time;
                float x = direc.x > 0 ? 1: -1;
                otter.setMove(x);
                Sensitivity = defaultSensitivity;
            }
            old_pos = transform.position;
            if (Time.time -  detected_time  > time_trace){
                otter.Sleep();
                is_Sleep = true;
                return;
            }

            if (Time.time -  detected_time > time_trace - 1){
                otter.setMove(0.1f);
                return;    
            }
            
            if (Sensitivity < 0){
                float x = UnityEngine.Random.Range(-0.5f,0.5f);
                otter.setMove(x);
                Sensitivity = defaultSensitivity;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Wolf_controller : MonoBehaviour
{
    // Start is called before the first frame update
    public wolf _wolf;
    private GameObject player = null;
    public Vector2 detect_size;
    public float Sensitivity;
    public float max_trace;
    public float time_trace;
    private float defaultSensitivity;
    private float _max_trace;
    private float last_time_jump;
    private Vector3 old_pos;
    void Start()
    {
        _max_trace = max_trace*max_trace;
        defaultSensitivity = Sensitivity;
        last_time_jump = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.BoxCast(gameObject.transform.position, detect_size,0,Vector2.zero,0,LayerMask.GetMask("Player"));
        
        if (hit.collider!= null && hit.collider.tag == "Player"){
            player = hit.collider.gameObject;
            Sensitivity = 0;
            old_pos = transform.position;
        }
        Sensitivity -= Time.deltaTime;
        old_pos = transform.position; 
        if (player == null){
            if (Sensitivity < 0){
                float x = UnityEngine.Random.Range(0.5f,1f);
                x = UnityEngine.Random.Range(-1,1) < 0 ? -x : x;
                _wolf.setMove(x);
                Sensitivity = defaultSensitivity;
            }
            return;
        }


        Vector2 direc = player.transform.position - gameObject.transform.position;

        if (direc.sqrMagnitude<3){
            _wolf.Attack();
        }
        if(MathF.Abs(direc.x) > Mathf.Abs(direc.y) && direc.sqrMagnitude < _max_trace && Sensitivity < 0 ){
            float x = direc.x > 0 ? 1: -1;
            _wolf.setMove(x);
            Sensitivity = defaultSensitivity;
        }       
        if (Sensitivity < 0){
                float x = UnityEngine.Random.Range(-0.8f,0.5f);
                x = UnityEngine.Random.Range(-1,1) < 0 ? -x : x;
                _wolf.setMove(x);
                Sensitivity = defaultSensitivity;
        }
    }
}

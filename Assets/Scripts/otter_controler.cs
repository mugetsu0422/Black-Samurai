using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Otter_controler : MonoBehaviour
{
    // Start is called before the first frame update
    public Otter otter;
    public Vector2 WakeUpSize;
    public float MaxDistanceTrace;
    public float GiveUpTime;

    public float MaxDistaceMoveIdle;
    private GameObject player = null;

    private bool is_Sleep;

    private float detected_time;
    private Tilemap map;
    private Vector3 start_point;
    void Start()
    {
        is_Sleep = true;
        if (otter != null)
            otter.Sleep();
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
        Vector3Int init =  map.WorldToCell(gameObject.transform.position);
        while ( !map.HasTile(init) ){
            init.y -=1;
        }
        init.y +=1;
        start_point = map.CellToWorld(init);
        Debug.Log(start_point);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(is_Sleep){
            RaycastHit2D hit = Physics2D.BoxCast(gameObject.transform.position, WakeUpSize,0,Vector2.zero,0,LayerMask.GetMask("Player"));
            
            if (hit.collider!= null && hit.collider.tag == "Player"){
                otter.Wake();
                is_Sleep = false;
                detected_time = Time.time;
                player = hit.collider.gameObject;
            }
        }
        else{
            Vector2 distance = player.transform.position - gameObject.transform.position; 
            if (distance.sqrMagnitude > MaxDistanceTrace*MaxDistanceTrace || MathF.Abs(distance.x) < MathF.Abs(distance.y)){
                Vector2 dis = start_point - gameObject.transform.position;
                if (dis.sqrMagnitude > MaxDistaceMoveIdle*MaxDistaceMoveIdle){
                    float x = dis.x > 0 ? 0.4f : -0.4f;
                    otter.setMove(x);
                }
            }
            else{
                float x = distance.x > 0 ? 1f : -1f;
                otter.setMove(x);
                detected_time = Time.time;
            }
            if (Time.time - detected_time > GiveUpTime - 2){
                otter.setMove(0);
            }
            if (Time.time - detected_time > GiveUpTime){
                otter.Sleep();
                is_Sleep = true;
                start_point = gameObject.transform.position;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonfire : MonoBehaviour
{
    // Start is called before the first frame update
    public string location_name;
    public bool isActive;

    void Start()
    {
        try{
            Save_Point.SavePoint savePoint =  Save_Point.savePointData.SavePoint(gameObject.scene.name,location_name);
            transform.GetChild(0).gameObject.SetActive(savePoint.isActive);
            isActive = savePoint.isActive;
        }
        catch(Exception){
            isActive = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame

    void FixedUpdate(){
        if(!isActive){
        RaycastHit2D hit = Physics2D.CircleCast(gameObject.transform.position,4f,Vector2.zero,LayerMask.GetMask("Player"));
            if (hit.collider.tag == "Player"){
                Active();
                Save_Point.savePointData.data[gameObject.scene.name][location_name].isActive = true;
                this.enabled = false;
            }
        }
    }


    public void Active(){
        isActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}

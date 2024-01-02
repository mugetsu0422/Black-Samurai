using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class bonfire : MonoBehaviour
{
    // Start is called before the first frame update
    public string location_name;
    public bool isActive;
    public GameObject tip;
    void Start()
    {
        tip.SetActive(false);
        try{
            Save_Point.SavePoint savePoint =  Save_Point.savePointData.SavePoint(gameObject.scene.name,location_name);
            transform.GetChild(0).gameObject.SetActive(savePoint.isActive);
            isActive = savePoint.isActive;
        }
        catch(Exception){
            isActive = false;
            transform.GetChild(0).gameObject.SetActive(false);
            Save_Point.SavePoint temp = new Save_Point.SavePoint();
            temp.name = location_name;
            temp.isActive = isActive;
            temp.location = gameObject.transform.position;
            temp.scene_name = gameObject.scene.name;
            Save_Point.savePointData.Add(temp);
        }
    }

    // Update is called once per frame

    void FixedUpdate(){
        tip.transform.position =  Camera.allCameras[0].WorldToScreenPoint(gameObject.transform.position+Vector3.up*6);
        RaycastHit2D hit = Physics2D.CircleCast(gameObject.transform.position,4f,Vector2.zero,LayerMask.GetMask("Player"));
        if (hit.collider.tag == "Player"){
            Active();
            tip.SetActive(true);
            Save_Point.savePointData.data[gameObject.scene.name][location_name].isActive = true;
        }
        else
            tip.SetActive(false);
    }


    public void Active(){
        isActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
        Save_Point.savePointData.SavePoint(gameObject.scene.name,location_name).isActive = true;
        Save_Point.savePointData.respawnPoint.map = gameObject.scene.name;
        Save_Point.savePointData.respawnPoint.location = transform.position;
    }
}

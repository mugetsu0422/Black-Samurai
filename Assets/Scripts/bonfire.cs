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

    public void Active(){
        isActive = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class initgame : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool loaded = false;
    void OnEnable()
    {
        if (loaded == false){
            Save_Point.savePointData = Save_Point.LoadData();
            loaded = true;
        }
        while (!loaded){}
        Save_Point.savePointData.Add(Save_Point.loadDataOnScene());
        Save_Point.SaveData(Save_Point.savePointData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

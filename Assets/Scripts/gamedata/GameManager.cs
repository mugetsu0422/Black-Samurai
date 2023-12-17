using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static void Save(){
            Save_Point.SaveData(Save_Point.savePointData);
    }
    public static void Load(){
            Save_Point.savePointData = Save_Point.LoadData();
    }
}

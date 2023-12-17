using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.SearchService;
using UnityEngine;

public static class Save_Point
{
    static string file_path = "data/bonfire.json";
    public static SavePointData savePointData = new SavePointData();

    [Serializable]
    public class SavePoint{
        public string scene_name;
        public string name ; 
        public Vector3 location;
        public bool isActive;
    }

    [Serializable]
    private class SavePointWriter{
        public List<SavePoint> data = new List<SavePoint>();
        public SavePointWriter(SavePointData _right){
            foreach(string i in _right.data.Keys){
                foreach(string k in _right.data[i].Keys){
                    data.Add(_right.data[i][k]);
                }
            }
        }

        public SavePointData toSavePointData(){
            SavePointData _data = new SavePointData();
            foreach(SavePoint i in data){
                if (!_data.data.ContainsKey(i.scene_name))
                    _data.data.Add(i.scene_name,new Dictionary<string, SavePoint>());
                _data.data[i.scene_name].Add(i.name,i);
            }
            return _data;
        }

    }

    [Serializable]
    public class SavePointData{
        //public List<SavePoint> data = new List<SavePoint>();
        public Dictionary<string,Dictionary<string,SavePoint>> data = new Dictionary<string, Dictionary<string, SavePoint>>();

        public SavePoint SavePoint(string scene,string location_name){
            try {
                return data[scene][location_name];
            }
            catch(Exception){
                return null;
            }
        }
        public List<SavePoint> SavePoint(string scene,bool activeOnly = false){
            try{
                if (activeOnly)
                    return data[scene].Values.ToList().FindAll(e => e.isActive == true);
                else
                    return data[scene].Values.ToList();
            }
            catch(Exception){
                return null;
            }
        }
        public List<string> getScenes(){
            try{
                return data.Keys.Distinct().ToList();
            }
            catch(Exception){
                return null;
            }
        }
        public void Add(SavePoint savePoint){
            if(data.ContainsKey(savePoint.scene_name)){
                if(data[savePoint.scene_name].ContainsKey(savePoint.name)){
                    data[savePoint.scene_name][savePoint.name] = savePoint;
                }
                else{
                    data[savePoint.scene_name].Add(savePoint.name,savePoint);
                }
            }
            else{
                data.Add(savePoint.scene_name,new Dictionary<string, SavePoint>());
                data[savePoint.scene_name].Add(savePoint.name,savePoint);
            }
        }
        public void Add(SavePointData _right,bool keep_old = true){
            foreach(string i in _right.data.Keys){
                if (!data.ContainsKey(i)){
                    data.Add(i,_right.data[i]);
                }
                else{
                    foreach(string k in _right.data[i].Keys){
                        try{
                            data[i].Add(k,_right.data[i][k]);
                        }
                        catch(Exception){
                            if (!keep_old)
                                data[i][k] = _right.data[i][k];
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame

    public static SavePointData LoadData(){
        try{
            string text = File.ReadAllText(file_path);
            return JsonUtility.FromJson<SavePointWriter>(text).toSavePointData();

        }
        catch(Exception){
            return new SavePointData();
        }
    }

    public static SavePointData loadDataOnScene(){
        SavePointData data = new SavePointData();
        GameObject [] bonfire = GameObject.FindGameObjectsWithTag("savepoint");
        foreach(GameObject i in bonfire){
                SavePoint temp = new SavePoint();
                temp.name = i.GetComponent<bonfire>().location_name;
                temp.isActive = i.GetComponent<bonfire>().isActive;
                temp.location = i.GetComponent<bonfire>().transform.position;
                temp.scene_name = i.scene.name;
                if (!data.data.ContainsKey(temp.scene_name))
                    data.data.Add(temp.scene_name,new Dictionary<string, SavePoint>());
                data.data[temp.scene_name].Add(temp.name,temp);
        }
        return data;
    }

    public static void SaveData(SavePointData data){
        SavePointWriter temp = new SavePointWriter(data);
        string data_s = JsonUtility.ToJson(temp);
        using (FileStream file = new FileStream(file_path,FileMode.Create)){
            using (StreamWriter write = new StreamWriter(file)){
                write.Write(data_s);
            }
        }
    }
}

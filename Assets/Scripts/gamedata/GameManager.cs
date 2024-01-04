using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private const string paths = "data/gamedata.json";
    [SerializeField]
    private class Data{
        public int pureParasiteHeart;
        public int health;
        public int ki;
        public int weaponLevel;
        public int parasiteEssence;
        public string scene;
        public Vector3 position;
        public List<String> boss = new List<String>();
        public List<bool> bossKilled = new List<bool>();
        public void Save(){
                string data_s = JsonUtility.ToJson(this);
                using (FileStream file = new FileStream(paths,FileMode.Create)){
                        using (StreamWriter write = new StreamWriter(file)){
                                write.Write(data_s);
                        }
                }
        }
        public static Data Load(){
                try{
                string text = File.ReadAllText(paths);
                return JsonUtility.FromJson<Data>(text);
                }
                catch(Exception){
                return null;
                }
        }
    }
    public static void Save(GameObject player){
        Data data = new Data();
        
        data.scene = player.scene.name;
        data.position = player.transform.position;

        data.pureParasiteHeart = KaguraBachiData.PureParasiteHeart;
        data.health = KaguraBachiData.Health;
        data.ki = KaguraBachiData.Ki;
        data.weaponLevel = KaguraBachiData.WeaponLevel;
        data.parasiteEssence = KaguraBachiData.ParasiteEssence;
        foreach(var i in KaguraBachiData.bossKilled){
                data.boss.Add(i.Key);
                data.bossKilled.Add(i.Value);
        }

        data.Save();
        Save_Point.SaveData(Save_Point.savePointData);
    }
    public static (string sceen, Vector3 Position) Load(){
        Save_Point.savePointData = Save_Point.LoadData();
        Data data = Data.Load();
        if (data == null){
                return (null,new Vector3());
        }
        KaguraBachiData.PureParasiteHeart = data.pureParasiteHeart;
        KaguraBachiData.Health = data.health;
        KaguraBachiData.Ki = data.ki;
        KaguraBachiData.WeaponLevel = data.weaponLevel;
        KaguraBachiData.ParasiteEssence = data.parasiteEssence;
        for(int i = 0 ; i < data.boss.Count ; i ++){
                try{
                KaguraBachiData.bossKilled.Add(data.boss[i],data.bossKilled[i]);
                }
                catch(Exception){}
        }

        return (data.scene,data.position);
    }
}

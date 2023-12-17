using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject UIGroup;
    public string[] scenePaths;
    private Dictionary<string,string> name_to_path;
    void Start(){
        name_to_path  = new Dictionary<string, string>();
        foreach (string i in scenePaths){
            string[] temp = i.Split('/');
            name_to_path.Add(temp[temp.Length-1].Split('.')[0],i);
        }
    }

    // Update is called once per frame
    public IEnumerator Teleport(string name,Vector3 position,GameObject[] keep = null)
    {
        string old_scene = gameObject.scene.name;
        if (old_scene != name){
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name_to_path[name], LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                yield return null;
            }
            UnityEngine.SceneManagement.Scene loadScene = SceneManager.GetSceneByName(name);

            if (keep == null){
                SceneManager.MoveGameObjectToScene(UIGroup, loadScene);
                SceneManager.MoveGameObjectToScene(player, loadScene);
            }
            else{
                foreach(GameObject i in keep)
                    SceneManager.MoveGameObjectToScene(i,loadScene);
            }
            try{
                SceneManager.UnloadSceneAsync(old_scene);
            }
            catch(Exception){}
        }
        else{

        }
        position.y+=5;
        player.transform.position= position;
    }
}

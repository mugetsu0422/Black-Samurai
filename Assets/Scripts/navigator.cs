using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class navigator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public string[] scenePaths;
    private Dictionary<string,string> name_to_path;
    public GameObject teleport_canvas;
    void Start(){
        name_to_path  = new Dictionary<string, string>();
        foreach (string i in scenePaths){
            string[] temp = i.Split('/');
            name_to_path.Add(temp[temp.Length-1].Split('.')[0],i);
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.T)){
            
            teleport_canvas.SetActive(!teleport_canvas.active);
        }
    }

    // Update is called once per frame
    public IEnumerator  Teleport(string name,Vector3 position)
    {
        string old_scene = gameObject.scene.name;
        if (old_scene != name){
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name_to_path[name], LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                yield return null;
            }
            UnityEngine.SceneManagement.Scene loadScene = SceneManager.GetSceneByName(name);
    
            teleport_canvas.SetActive(false);
            SceneManager.MoveGameObjectToScene(gameObject, loadScene);
            SceneManager.MoveGameObjectToScene(teleport_canvas, loadScene);
            try{
                SceneManager.UnloadScene(old_scene);
            }
            catch(Exception){}
        }
        else{
            teleport_canvas.SetActive(false);
        }
        position.y+=5;
        GameObject.FindWithTag("Player").transform.position= position;

    }
}

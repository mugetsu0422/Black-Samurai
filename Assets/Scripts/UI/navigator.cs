using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject UIGroup;
    public string[] scenePaths;
    [SerializeField] GameObject loadingScreen;
    private Dictionary<string,string> name_to_path;
    void Start(){
        name_to_path  = new Dictionary<string, string>();
        foreach (string i in scenePaths){
            string[] temp = i.Split('/');
            name_to_path.Add(temp[temp.Length-1].Split('.')[0],i);
        }
    }

    // Update is called once per frame
    public IEnumerator Teleport(string name,Vector3 position,GameObject[] keep = null,GameObject close_afterload = null)
    {
        string old_scene = gameObject.scene.name;
        if (old_scene != name){
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name_to_path[name], LoadSceneMode.Additive);
            LoadingScreenProgressBar.Instance.SetValue(0f);
            loadingScreen.SetActive(true);
            LoadingScreenProgressBar.Instance.isLoading = true;
            while (!asyncLoad.isDone) {
                LoadingScreenProgressBar.Instance.SetValue(asyncLoad.progress);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(1f);
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
            LoadingScreenProgressBar.Instance.SetValue(1f);
            loadingScreen.SetActive(false);
            LoadingScreenProgressBar.Instance.isLoading = false;
        }
        else{

        }
        position.y+=5;
        player.transform.position= position;
        if (close_afterload != null)
            close_afterload.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NewGameBar;
    public GameObject ContinueBar;
    public GameObject SettingsBar;
    public GameObject QuitBar;
    public GameObject ChossingBar;
    public Vector3 init_player_index;
    public GameObject SettingBarUI;
    public GameObject player;
    public GameObject UIGroup;
    public UIController uIController;
    private int chossing;
    private List<GameObject> Bars = new List<GameObject>();
    
    void Awake()
    {   
        chossing = 0;
        Bars.Add(NewGameBar);
        Bars.Add(ContinueBar);
        Bars.Add(SettingsBar);
        Bars.Add(QuitBar);
        chossing = 0;
        var temp = ChossingBar.transform.position;
        temp.y = Bars[chossing].transform.position.y;
        ChossingBar.transform.position =  temp;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&chossing < 3){
            chossing += 1;;
            var temp = ChossingBar.transform.position;
            temp.y = Bars[chossing].transform.position.y;
            ChossingBar.transform.position =  temp;
        }
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && chossing > 0){
            chossing -= 1;
            var temp = ChossingBar.transform.position;
            temp.y = Bars[chossing].transform.position.y;
            ChossingBar.transform.position =  temp;
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            if (chossing == 0){
                StartCoroutine(NewGame());
            }  
            else if (chossing == 1){
                StartCoroutine(ContinueGame());
            }
            else if (chossing == 2){
                SettingBarUI.SetActive(true);
                gameObject.SetActive(false);
            }
            else if (chossing == 3){
                Application.Quit();
            }
        }
    }

    public IEnumerator NewGame(){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {yield return null;}
        UnityEngine.SceneManagement.Scene loadScene = SceneManager.GetSceneByBuildIndex(1);
        SceneManager.MoveGameObjectToScene(UIGroup,loadScene);
        SceneManager.MoveGameObjectToScene(player,loadScene);
        player.SetActive(true);
        player.transform.position = init_player_index;
        uIController.enabled = true;
        try{
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
        }
        catch(Exception){}
    }

    public  IEnumerator ContinueGame(){
        string scene;
        Vector3 player_pos;
        (scene,player_pos) = GameManager.Load();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {yield return null;}
        UnityEngine.SceneManagement.Scene loadScene = SceneManager.GetSceneByName(scene);
        SceneManager.MoveGameObjectToScene(UIGroup,loadScene);
        SceneManager.MoveGameObjectToScene(player,loadScene);
        player.SetActive(true);
        player.transform.position = player_pos;
        uIController.enabled = true;
        try{
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
        }
        catch(Exception){}
    }
}

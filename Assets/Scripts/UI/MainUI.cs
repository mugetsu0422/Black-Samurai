using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject NewGameBar;
    public GameObject ContinueBar;
    public GameObject SettingsBar;
    public GameObject QuitBar;
    public GameObject ChossingBar;

    public GameObject SettingBarUI;
    private navigator nav = new navigator();
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
                //
            }
            else if (chossing == 1){
                GameManager.Load();
                // screen = Game character data.scene;
                // sceen  
                //nav.Teleport()
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
}

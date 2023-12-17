using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Vector2Int> Resolutions = new List<Vector2Int>();
    public GameObject ChossingBar;
    public GameObject ResolutionText;
    public GameObject FullScreenText;
    public GameObject SoundText;
    public GameObject SaveText;
    public Slider Volume;
    public Text res;
    public Text full_screen;

    public GameObject returnUI;
    private int chossing;
    private int res_chossing;
    private bool is_full;
    private List<GameObject> bars = new List<GameObject>();
    public static class GameSettings{
        public static Vector2Int resolution = new Vector2Int(1280,720);
        public static bool is_fullscreen = false;
        public static float volume = 1f;
    }

    void Awake(){
        Resolutions.Add(new Vector2Int(1280,720));
        Resolutions.Add(new Vector2Int(1366,768));
        Resolutions.Add(new Vector2Int(1920,1080));
        Resolutions.Add(new Vector2Int(2560,1440));
        bars.Add(ResolutionText);
        bars.Add(FullScreenText);
        bars.Add(SoundText);
        bars.Add(SaveText);
    }
    void OnEnable()
    {
        chossing = 0;
        for(int i = 0; i < Resolutions.Count; i++){
            if (Resolutions[i] == GameSettings.resolution)
                res_chossing = i;
        }
        res.text = "\t< "+ Resolutions[res_chossing].x + "x" + Resolutions[res_chossing].y + " >"; 
        is_full = GameSettings.is_fullscreen;
        full_screen.text = is_full? "\tYes": "\tNo";
        ChossingBar.transform.position = new Vector2(ChossingBar.transform.position.x,bars[chossing].transform.position.y);
        Volume.value = GameSettings.volume;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            gameObject.SetActive(false);
            try
            {
                returnUI.SetActive(true);
            }
            catch (System.Exception){}
        }
        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&chossing < 3){
            chossing +=1;
            ChossingBar.transform.position = new Vector2(ChossingBar.transform.position.x,bars[chossing].transform.position.y);
        }
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) &&chossing > 0){
            chossing -=1;
            ChossingBar.transform.position = new Vector2(ChossingBar.transform.position.x,bars[chossing].transform.position.y);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)){
            if (chossing == 0){
                res_chossing = res_chossing == 0 ? res_chossing : res_chossing - 1;
                res.text = "\t< "+ Resolutions[res_chossing].x + "x" + Resolutions[res_chossing].y + " >"; 
            }
            if (chossing == 1){
                is_full = !is_full;
                full_screen.text = is_full? "\tYes": "\tNo";
            }
            if (chossing == 2){
                Volume.value -=0.1f;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)){
            if (chossing == 0){
                res_chossing = res_chossing == Resolutions.Count-1 ?res_chossing: res_chossing +1;
                res.text = "\t< "+ Resolutions[res_chossing].x + "x" + Resolutions[res_chossing].y + " >"; 
            }
            if (chossing == 1){
                is_full = !is_full;
                full_screen.text = is_full? "\tYes": "\tNo";
            }
            if (chossing == 2){
                Volume.value +=0.1f;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && chossing == 3){
            GameSettings.resolution = Resolutions[res_chossing];
            GameSettings.volume = Volume.value;
            GameSettings.is_fullscreen = is_full;
            Screen.SetResolution(GameSettings.resolution.x,GameSettings.resolution.y,GameSettings.is_fullscreen);
            gameObject.SetActive(false);
            try
            {
                returnUI.SetActive(true);
            }
            catch (System.Exception){}

        }
    }
}

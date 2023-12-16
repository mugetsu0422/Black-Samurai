using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ContinueText;
    public GameObject SettingText;
    public GameObject ExitText;
    public GameObject ChooseBar;
    public GameObject SetingUI;
    private int chossing;

    private List<GameObject> bars = new List<GameObject>();
    void Awake(){
        bars.Add(ContinueText);
        bars.Add(SettingText);
        bars.Add(ExitText);
    }
    void OnEnable()
    {
        chossing = 0;
        Vector3 temp = ChooseBar.transform.position;
        temp.y = bars[chossing].transform.position.y;
        ChooseBar.transform.position = temp; 
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) &&chossing < 2){
            chossing +=1;
            ChooseBar.transform.position = new Vector2(ChooseBar.transform.position.x,bars[chossing].transform.position.y);
        }
        if((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) &&chossing > 0){
            chossing -=1;
            ChooseBar.transform.position = new Vector2(ChooseBar.transform.position.x,bars[chossing].transform.position.y);
        }

        if(Input.GetKeyDown(KeyCode.Return)){
            if (chossing == 0){
                gameObject.SetActive(false);
                Time.timeScale = 1;
            }
            if(chossing == 1){
                gameObject.SetActive(false);
                SetingUI.SetActive(true);
            }
            if (chossing == 2){
                GameManager.Save();
                Time.timeScale = 1;
                gameObject.SetActive(false);
                //return main screen
            }
        }
    }
}

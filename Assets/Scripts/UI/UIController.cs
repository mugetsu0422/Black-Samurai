using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Pause;
    public GameObject Teleport;

    // Update is called once per frame
    void Update()
    {
        if (Pause.activeSelf == Teleport.activeSelf && Pause.activeSelf == false){
            if (Input.GetKeyDown(KeyCode.T)){
                Teleport.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.Escape)){
                Pause.SetActive(true);
            }
        }
        else{
            if (Input.GetKeyDown(KeyCode.T)){
                Teleport.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Escape)){
                Pause.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}

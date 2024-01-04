using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                List<Save_Point.SavePoint> saves = Save_Point.savePointData.SavePoint(gameObject.scene.name);
                foreach(Save_Point.SavePoint i in saves){
                    if ( (player.transform.position - i.location).sqrMagnitude < 16f){
                        Teleport.SetActive(true);
                        return;
                    }
                }
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
        if (Teleport.activeSelf == true){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            List<Save_Point.SavePoint> saves = Save_Point.savePointData.SavePoint(gameObject.scene.name);
            bool setactive = false;
            foreach(Save_Point.SavePoint i in saves){
                if ( (player.transform.position - i.location).sqrMagnitude < 16f){
                    setactive = true;
                }
            }
            Teleport.SetActive(setactive);
        }
    }
}

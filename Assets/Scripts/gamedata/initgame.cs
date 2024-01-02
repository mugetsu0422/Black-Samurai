using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class initgame : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 player_scale; 
    void OnEnable()
    {
        Screen.SetResolution(Setting.GameSettings.resolution.x,Setting.GameSettings.resolution.y,Setting.GameSettings.is_fullscreen);
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindWithTag("Player");
        if (player!=null){
            player.transform.localScale = player_scale;
            enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class initgame : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool loaded = false;
    void OnEnable()
    {
        Screen.SetResolution(Setting.GameSettings.resolution.x,Setting.GameSettings.resolution.y,Setting.GameSettings.is_fullscreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

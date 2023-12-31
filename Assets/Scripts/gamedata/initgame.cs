using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class initgame : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 player_scale; 
    public float speed;
    public float jumpHeight;
    void OnEnable()
    {
        Screen.SetResolution(Setting.GameSettings.resolution.x,Setting.GameSettings.resolution.y,Setting.GameSettings.is_fullscreen);
    }

    // Update is called once per frame
    void Update()
    {
        var player = GameObject.FindWithTag("Player");
        var cam = GameObject.FindWithTag("MainCamera");
        if (player!=null && cam !=null){
            player.transform.localScale = player_scale;
            cam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
            CharacterScript characterScript = player.GetComponent<CharacterScript>();

            if (characterScript != null)
            {
                // Set the speed and jumpHeight values
                characterScript.speed = speed; // Change this value to the desired speed
                characterScript.jumpHeight = jumpHeight; // Change this value to the desired jump height
            }
        }
    }
}

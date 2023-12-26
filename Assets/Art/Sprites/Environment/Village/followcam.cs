using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followcam : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.transform.position;
        pos.z = Camera.main.transform.position.z;
        Camera.main.transform.position = pos;
        gameObject.transform.position = cam.transform.position ;
    }
}

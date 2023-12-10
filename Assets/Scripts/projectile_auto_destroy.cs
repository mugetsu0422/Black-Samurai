using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile_auto_destroy : MonoBehaviour
{
    // Start is called before the first frame update

    public float destroy_time;
    void Awake()
    {
        Destroy(gameObject,destroy_time);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

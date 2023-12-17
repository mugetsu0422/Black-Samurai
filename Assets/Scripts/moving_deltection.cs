using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_deltection : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rgbd;
    public LineRenderer line;
    private float delta_time = 0.02f;
    void Start()
    {
        line.positionCount=100;
    }

    // Update is called once per frame
    void Update()
    { 
    }
}

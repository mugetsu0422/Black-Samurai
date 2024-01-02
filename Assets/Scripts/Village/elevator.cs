using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elevator : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 start;
    public Vector3 end;
    public float speed;
    public Vector3 direction;
    private bool toEnd;
    private float travelDistance;
    void Start()
    {
        gameObject.transform.position = start;
        direction = end - start;
        direction.Normalize();
        toEnd = true;
        travelDistance =  (end - start).sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if (toEnd){
            gameObject.transform.position += direction * Time.deltaTime * speed;
            if ((gameObject.transform.position - start).sqrMagnitude > travelDistance){
                toEnd = false;
                direction = start - end;
                direction.Normalize();
            }
        }
        else{
            gameObject.transform.position += direction * Time.deltaTime * speed;
            if ((gameObject.transform.position - end).sqrMagnitude > travelDistance ){
                toEnd = true;
                direction = end - start;
                direction.Normalize();
            }
        }
    }
}

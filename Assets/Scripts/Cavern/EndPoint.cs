using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public GameObject end;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        end.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collide with player
        if (other.CompareTag("Player"))
        {
            end.SetActive(true);
            BackgroundMusic.instance.creditBGM();
        }
    }
}

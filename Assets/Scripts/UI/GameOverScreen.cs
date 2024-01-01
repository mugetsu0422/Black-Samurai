using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }
    bool isOpen = false;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && isOpen)
        {
            CloseScreen();
        }
    }

    public void OpenScreen()
    {
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void CloseScreen()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }
}

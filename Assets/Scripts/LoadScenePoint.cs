using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScenePoint : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] Vector2 startPosition;
    Navigator navigator;

    private void Start()
    {
        var temp = GameObject.Find("InGameUI_group");
        if (temp)
        {
            navigator = temp.transform.Find("navigator").GetComponent<Navigator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(navigator.Teleport(sceneName, startPosition));
        }
    }
}

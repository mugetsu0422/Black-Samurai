using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScenePoint : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.LoadScene(sceneName);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] Camera cameraTransform;

    void Start()
    {
        // Find the main camera in the scene
        transform.localScale *= cameraTransform.orthographicSize - 5;
    }

    void Update()
    {
        // Sync the background's position with the camera's position
        transform.position = new Vector3(cameraTransform.transform.position.x, cameraTransform.transform.position.y - 5, transform.position.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Find the main camera in the scene
        cameraTransform = Camera.main.transform;
        transform.localScale *= Camera.main.orthographicSize - 5;
    }

    void Update()
    {
        // Sync the background's position with the camera's position
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y - 5, transform.position.z);
    }
}

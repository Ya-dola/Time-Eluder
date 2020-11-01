using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Enabling on the fly camera speed changes for debugging
    public int cameraSpeed { get; set; }

    private void FixedUpdate()
    {
        if (GameManager.singleton.GameStarted)
            transform.position = transform.position + Vector3.forward * cameraSpeed * Time.fixedDeltaTime;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // Enabling on the fly camera speed changes for debugging
    public int backgroundSpeed { get; set; }

    private void FixedUpdate()
    {
        if (GameManager.singleton.GameStarted)
            transform.position = transform.position + Vector3.forward * backgroundSpeed * Time.fixedDeltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashedLineController : MonoBehaviour
{
    public Rigidbody dashedLineRigBody;

    private void FixedUpdate()
    {
        // Make the player move when game starts 
        if (GameManager.singleton.GameStarted)
            dashedLineRigBody.MovePosition(transform.position + Vector3.forward * GameManager.singleton.environmentWalkingSpeed * Time.fixedDeltaTime);
    }
}

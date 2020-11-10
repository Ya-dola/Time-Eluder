using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        // To check if the player collided with the coin
        PlayerController playerController = collider.GetComponent<PlayerController>();

        if (!playerController)
            return;

        GameManager.singleton.EndGame(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        PlayerController playerController = collider.GetComponent<PlayerController>();

        if (!playerController || GameManager.singleton.GameEnded)
            return;

        GameManager.singleton.EndGame(true);
    }
}

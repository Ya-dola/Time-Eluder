using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public AudioClip playerCollisionSound;
    [Range(0, 1)]
    public float playerCollisionSoundVolume;

    private void OnTriggerEnter(Collider collider)
    {
        // To check if the player collided with the coin
        PlayerController playerController = collider.GetComponent<PlayerController>();

        if (!playerController)
            return;

        // Plays the sound between the Camera's position and the Obstacle's position
        AudioSource.PlayClipAtPoint(playerCollisionSound, 0.9f * Camera.main.transform.position + 0.1f * transform.position, playerCollisionSoundVolume);

        // Ends the game after Collision and Shows that player Lost
        GameManager.singleton.EndGame(false);
    }
}

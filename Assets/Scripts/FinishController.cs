using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    public AudioClip gameWonSound;
    [Range(0, 1)]
    public float gameWonSoundVolume;

    private void OnTriggerEnter(Collider collider)
    {
        PlayerController playerController = collider.GetComponent<PlayerController>();

        if (!playerController || GameManager.singleton.GameEnded)
            return;

        GameManager.singleton.EndGame(true);

        // Plays the sound between the Camera's position and the Finish Line's position
        AudioSource.PlayClipAtPoint(gameWonSound, 0.9f * Camera.main.transform.position + 0.1f * transform.position, gameWonSoundVolume);
    }
}

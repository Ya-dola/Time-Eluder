using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public AudioClip coinCollectSound;
    [Range(0, 1)]
    public float coinCollectSoundVolume;

    private void OnTriggerEnter(Collider collider)
    {
        // To check if the player collided with the coin
        PlayerController playerController = collider.GetComponent<PlayerController>();

        if (!playerController)
            return;

        GameManager.singleton.AddCoinCollected();

        // Plays the sound between the Camera's position and the Coin's position
        AudioSource.PlayClipAtPoint(coinCollectSound, 0.9f * Camera.main.transform.position + 0.1f * transform.position, coinCollectSoundVolume);
        
        // Destroys the Coin after Collision
        Destroy(gameObject);
    }
}

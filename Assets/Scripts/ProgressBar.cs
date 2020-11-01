using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private float lastBarValue;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        particleSystem = GameObject.Find("Progress Bar Particles").GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checking if game running
        if (!GameManager.singleton.GameStarted)
            return;

        float travelledDistance = GameManager.singleton.EntireDistance - GameManager.singleton.RemainingDistance;
        float barValue = travelledDistance / GameManager.singleton.EntireDistance;

        // If the progress bar is not to supposed to increase
        if (GameManager.singleton.GameEnded && barValue < lastBarValue)
        {
            particleSystem.Stop();
            return;
        }

        slider.value = Mathf.Lerp(slider.value, barValue, 5 * Time.deltaTime);

        if (!particleSystem.isPlaying)
            particleSystem.Play();

        lastBarValue = barValue;
    }

    // Add Progress to the Bar
    // public void 
}

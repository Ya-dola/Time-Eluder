using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;
    private float lastBarValue;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
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

        float travelledDistance = GameManager.singleton.entireDistance - GameManager.singleton.remainingDistance;
        float barValue = travelledDistance / GameManager.singleton.entireDistance;

        // If the progress bar is not to supposed to increase
        if (GameManager.singleton.GameEnded && barValue < lastBarValue)
            return;

        slider.value = Mathf.Lerp(slider.value, barValue, 5 * Time.deltaTime);

        lastBarValue = barValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StartManager : MonoBehaviour
{
    public GameObject startButton;
    public GameObject loadingBar;
    public Slider loadingBarSlider;

    public TextMeshProUGUI loadingBarPercentageText;

    public void StartTimeEluder()
    {
        StartCoroutine(LoadSceneAsync("Game Scene"));
    }

    public void QuitTimeEluder()
    {
        // Written to show as Application.Quit doesnt do anything in Editor
        Debug.Log("Quit the Game !!!");

        Application.Quit();
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Scene Numbers are according to those shown in Build
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);

        startButton.SetActive(false);
        loadingBar.SetActive(true);

        while (!loadSceneOperation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(loadSceneOperation.progress / 0.9f);

            loadingBarSlider.value = loadingProgress;
            loadingBarPercentageText.text = loadingProgress * 100 + "%";

            yield return null;
        }
    }
}

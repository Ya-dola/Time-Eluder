using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public void StartTimeEluder()
    {
        // Scene Numbers are according to those shown in Build
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
    }

    public void QuitTimeEluder()
    {
        // Written to show as Application.Quit doesnt do anything in Editor
        Debug.Log("Quit the Game !!!");

        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameWon { get; private set; }
    public bool GameLost { get; private set; }
    public bool GameEnded { get; private set; }
    public bool GamePaused { get; private set; }
    public bool DashStatus { get; private set; }
    public bool SlowTimeStatus { get; private set; }

    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    public float playerSpeedFactor;

    [Header("Camera")]
    public CameraController cameraController;
    public float minCamDistance;
    public float maxCamDistance;

    [Header("Camera Background")]
    public BackgroundController backgroundController;

    [Header("Time Flow")]
    public float defaultTimeFlow;
    public float slowTimeFlow;
    public float slowMotionFactor;
    public float deltaTime;
    public int transitionTime;

    [Header("Abilities")]
    public Image darkDashImage;
    public float dashIncrement;
    public float dashCooldown;
    public TextMeshProUGUI dashCountText;
    public int dashCount { get; set; }
    public bool isDashCooldown { get; set; }
    public Image darkSlowTimeImage;
    public Image activeSlowTimeImage;
    public GameObject slowTimeBackdrop;
    public float slowTimeIncrement;
    public float slowTimeCooldown;
    public TextMeshProUGUI slowTimeCountText;
    public int slowTimeCount { get; set; }
    public bool isSlowTimeCooldown { get; set; }

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    public int currentScore { get; set; }
    public int highScore { get; set; }
    public int coinScoreValue;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public Toggle dashedLineToggle;

    [Header("End Game Menu")]
    public GameObject joyStickCanvas;
    public GameObject cameraCanvas;
    public GameObject gameWonMenu;
    public TextMeshProUGUI gameWonScoreText;
    public GameObject gameLostMenu;
    public TextMeshProUGUI gameLostScoreText;

    [Header("Enviroment")]
    public float sideWallDistance;
    public int environmentWalkingSpeed;
    public Transform startLine;
    public Transform finishLine;
    public GameObject dashedLines;

    // Player Distance Management
    public float entireDistance { get; private set; }
    public float remainingDistance { get; private set; }

    private void Awake()
    {
        // Creates a Single Instance of the game manager through out the entire game
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        // Specifies Default Time Flow
        Time.timeScale = defaultTimeFlow;
        Time.fixedDeltaTime = deltaTime;

        // Environment Initial Walking Speed
        cameraController.cameraSpeed = environmentWalkingSpeed;
        backgroundController.backgroundSpeed = environmentWalkingSpeed;

        // TODO - Load the saved high score
        // highScore = PlayerPrefs.GetInt("HighScore");
    }

    void Start()
    {
        // Progress Bar
        entireDistance = finishLine.position.z - startLine.position.z;

        // Abilities Cooldown 
        darkDashImage.fillAmount = 0;
        darkSlowTimeImage.fillAmount = 0;
        activeSlowTimeImage.fillAmount = 0;

        // Score Value
        currentScore = 0;

        // Abilities Values
        dashCount = 0;
        slowTimeCount = 0;
    }

    void Update()
    {
        remainingDistance = Vector3.Distance(playerController.transform.position, finishLine.position);

        // If Player is behind start line then the distance is the distance between the start and finish lines 
        if (remainingDistance > entireDistance)
            remainingDistance = entireDistance;

        // To avoid negative distance being shown to the player if the Player has passed the finish line
        if (playerController.transform.position.z > finishLine.transform.position.z)
            remainingDistance = 0;

        // Score Updates
        scoreText.text = "Score: " + currentScore;

        // Dash Count Updates
        dashCountText.text = "" + dashCount;

        // Slow Time Count Updates
        slowTimeCountText.text = "" + slowTimeCount;
    }

    public void AddCoinCollected()
    {
        // Score Update on collecting a coin
        currentScore += coinScoreValue;

        // Dash Count Increases every 4 coins collected
        if (currentScore % dashIncrement == 0)
            dashCount++;

        // Slow Time Count Increases every 12 coins collected
        if (currentScore % slowTimeIncrement == 0)
            slowTimeCount++;

        if (currentScore > highScore)
        {
            // TODO - To store the new high score for the user
            // PlayerPrefs.SetInt("HighScore", highScore);
            highScore = currentScore;
        }
    }

    public void StartGame()
    {
        GameStarted = true;

        // Plays Player Running Sound in the background
        playerController.playerRunningAudioSource.Play();
    }

    public void PauseOrResumeGame()
    {
        GamePaused = !GamePaused;

        pauseMenu.SetActive(GamePaused);

        if (GamePaused)
        {
            Time.timeScale = 0f;

            // Pauses Player Running Sound in the background
            playerController.playerRunningAudioSource.Pause();
        }
        else
        {
            Time.timeScale = 1f;

            // Plays Player Running Sound in the background
            playerController.playerRunningAudioSource.Play();
        }
    }

    public void EndGame(bool gameWon)
    {
        GameEnded = true;

        // Stops Player Running Sound in the background
        playerController.playerRunningAudioSource.Stop();

        // Disables the Joystick and hides it
        joyStickCanvas.SetActive(false);

        // Hides the Camera Canvas UI
        cameraCanvas.SetActive(false);

        if (gameWon)
        {
            GameWon = true;
            AddSlowMotionEffect("ShowGameWonScreen", transitionTime);
        }
        else
        {
            GameLost = true;
            AddSlowMotionEffect("ShowGameLostScreen", transitionTime);
        }
    }

    private void ShowGameWonScreen()
    {
        Time.timeScale = 0f;

        gameWonScoreText.text = "Score: " + currentScore;
        gameWonMenu.SetActive(true);
    }

    private void ShowGameLostScreen()
    {
        Time.timeScale = 0f;

        gameLostScoreText.text = "Score: " + currentScore;
        gameLostMenu.SetActive(true);
    }

    public void QuitGame()
    {
        // Written to show as Application.Quit doesnt do anything in Editor
        // Debug.Log("Quit the Game !!!");

        Application.Quit();
    }

    public void RestartGame()
    {
        // Scene Numbers are according to those shown in Build
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
    }

    public void GoToNextLevel()
    {
        // TODO - Add logic to increase length according to a variable when the game scene loads according to level number
        // Debug.Log("Loading Next Level !!!");

        // Temporary Implementation
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Scene");
    }

    public void DashedLineToggleChanged()
    {
        if (dashedLineToggle.isOn)
            dashedLines.transform.Translate(0, -100f, 0);
        else
            dashedLines.transform.Translate(0, 100f, 0);

        // TODO - Add dashedLineToggle.isOn to Player Preferences
    }

    private void AddSlowMotionEffect(string method, int slowMotionTime)
    {
        // Calls Method after Specified time
        Invoke(method, slowMotionTime * slowMotionFactor);

        // Creates Slow motion time flow effect 
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = deltaTime * Time.timeScale;
    }

    public void StartSlowTimeFlow()
    {
        Time.timeScale = slowTimeFlow;
        slowTimeBackdrop.SetActive(true);
    }

    public void EndSlowTimeFlow()
    {
        Time.timeScale = defaultTimeFlow;
        slowTimeBackdrop.SetActive(false);
    }

    public void DashStarted()
    {
        DashStatus = true;
    }

    public void DashEnded()
    {
        DashStatus = false;
    }

    public void SlowTimeStarted()
    {
        SlowTimeStatus = true;
    }

    public void SlowTimeEnded()
    {
        SlowTimeStatus = false;
    }
}

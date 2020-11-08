﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    public float playerSpeedFactor;

    [Header("Camera")]
    public CameraController cameraController;
    public float minCamDistance;
    public float maxCamDistance;
    
    [Header("Camera Background")]
    public BackgroundController backgroundController;

    [Header("Slow Motion Time")]
    [SerializeField] private float slowMotionFactor = 0.1f;
    [SerializeField] private float deltaTime = 0.02f;
    [SerializeField] private int transitionTime = 3;

    // Abilities Management
    [Header("Abilities")]
    public Image darkDashImage;
    public float playerDashCooldown;
    public bool isDashCooldown { get; set; }
    public Image darkSlowTimeImage;
    public float playerSlowTimeCooldown;
    public bool isSlowTimeCooldown { get; set; }

    [Header("Score")]
    public TextMeshProUGUI scoreText;
    public int currentScore { get; set; }
    public int highScore { get; set; }
    public int coinScoreValue;

    // [Header("UI Text")]
    // public GameObject youWonText;
    // public GameObject youDiedText;
    
    [Header("Enviroment")]
    public float sideWallDistance;
    public int environmentWalkingSpeed;
    public Transform startLine;
    public Transform finishLine;

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

        // Specifies Default time flow
        Time.timeScale = 1f;
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

        // Score Value
        currentScore = 0;

        // Signals the Game has started and only runs it once if the game has already started
        if (!GameManager.singleton.GameStarted)
            GameManager.singleton.StartGame();

        // youWonText.SetActive(false);
        // youDiedText.SetActive(false);
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
    }

    public void AddCoinCollected()
    {
        // Score Update on collecting a coin
        currentScore += coinScoreValue;

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

        // TODO - Change to have a Button in UI to start the game
        // Base the buttons off of what was used in Grid Generator
        // if (Input.GetKeyDown("w"))
        // {
        //     // Signals the Game has started and only runs it once if the game has already started
        //     if (!GameManager.singleton.GameStarted)
        //         GameManager.singleton.StartGame();
        //     Debug.Log("Game Started !!!");
        // }

    }

    // TODO - Change how this works to be easier to reuse
    public void EndGame(bool gameWon)
    {
        GameEnded = true;

        if (!gameWon)
        {
            AddSlowMotionEffect("RestartGame", transitionTime);

            // Debug.Log("Death Obstracle hit !!!");
            // youDiedText.SetActive(true);
        }
        else
        {
            Invoke("RestartGame", transitionTime);

            // Debug.Log("Finish Line Reached !!!");
            // youWonText.SetActive(true);
        }
    }

    public void RestartGame()
    {
        // Scene Numbers are according to those shown in Build
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void AddSlowMotionEffect(string method, int slowMotionTime)
    {
        // Calls Method after Specified time
        Invoke(method, slowMotionTime * slowMotionFactor);

        // Creates Slow motion time flow effect 
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = deltaTime * Time.timeScale;
    }
}

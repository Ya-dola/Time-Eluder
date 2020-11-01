using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    [Header("Start and Finish Lines")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform finishTransform;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

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

    // [Header("UI Text")]
    // public GameObject youWonText;
    // public GameObject youDiedText;

    // Player Distance Management
    public float EntireDistance { get; private set; }
    public float RemainingDistance { get; private set; }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        // Specifies Default time flow
        Time.timeScale = 1f;
        Time.fixedDeltaTime = deltaTime;
    }

    void Start()
    {
        EntireDistance = finishTransform.position.z - startTransform.position.z;

        darkDashImage.fillAmount = 0;
        darkSlowTimeImage.fillAmount = 0;

        // Signals the Game has started and only runs it once if the game has already started
        if (!GameManager.singleton.GameStarted)
            GameManager.singleton.StartGame();

        // youWonText.SetActive(false);
        // youDiedText.SetActive(false);
    }

    void Update()
    {
        RemainingDistance = Vector3.Distance(playerController.transform.position, finishTransform.position);

        // If Player is behind start line then the distance is the distance between the start and finish lines 
        if (RemainingDistance > EntireDistance)
            RemainingDistance = EntireDistance;

        // To avoid negative distance being shown to the player if the Player has passed the finish line
        if (playerController.transform.position.z > finishTransform.transform.position.z)
            RemainingDistance = 0;
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
